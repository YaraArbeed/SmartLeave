using AutoMapper;
using EmailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using SmartLeave.API.DTOs;
using SmartLeave.API.JwtFeaturs;
using SmartLeave.BLL.Interfaces;
using SmartLeave.DAL.Entities;

namespace SmartLeave.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        //Map Incoming DTO Object To the user Object
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly JwtHandler _jwtHandler;
        private readonly IEmailSender _emailSender;
        private readonly ILeaveBalanceService _balanceService;

        public AccountsController(IMapper mapper, UserManager<User> userManager, JwtHandler jwtHandler, IEmailSender emailSender, ILeaveBalanceService balanceService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _jwtHandler = jwtHandler;
            _emailSender = emailSender;
            _balanceService = balanceService;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            if (userForRegistration is null)
                return BadRequest();

            if (userForRegistration.Role == "Admin")
                return BadRequest("You are not allowed to register as an Admin.");

            // 1. Map the UserForRegistrationDto to the ApplicationUser entity
            var user = _mapper.Map<User>(userForRegistration); // Assuming ApplicationUser is your Identity user class

            // 2. Create the user using UserManager
            // The '!' (null-forgiving operator) on Password assumes it's not null due to prior validation.
            var result = await _userManager.CreateAsync(user, userForRegistration.Password!);

            // 3. Handle user creation errors
            if (!result.Succeeded)
            {
                // Extract error descriptions from the IdentityResult
                var errors = result.Errors.Select(e => e.Description);

                // Return a Bad Request response with the registration errors
                 return BadRequest(new RegistrationResponseDto { Errors = errors });

            }
            user.EmailConfirmed = true;
            user.CountryId = userForRegistration.CountryId;
            user.OfficeId = userForRegistration.OfficeId;
            user.Position= userForRegistration.Position;
            user.DateHired=userForRegistration.DateHired;
            await _userManager.UpdateAsync(user);
            await _userManager.SetTwoFactorEnabledAsync(user, true);

            await _userManager.AddToRoleAsync(user, userForRegistration.Role);
            await _balanceService.InitializeForUserAsync(user);
            return StatusCode(201);
          
            
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto userForAuthentication)
        {
            // 1. Find the user by email
            // Using FindByEmailAsync is often preferred over FindByNameAsync if email is the username
            var user = await _userManager.FindByEmailAsync(userForAuthentication.Email);

            // 2. Check if user exists and password is correct
            if (user is null)
            {
                return BadRequest("Invalid Request");
            }



            if (await _userManager.IsLockedOutAsync(user))
            {
                return Unauthorized(new AuthResponseDto { ErrorMessage = "The account is locked out" });
            }

            if (!await _userManager.CheckPasswordAsync(user, userForAuthentication.Password))
            {
                await _userManager.AccessFailedAsync(user);
                if (await _userManager.IsLockedOutAsync(user))
                {
                    var content = $"Your account is locked out. If you want to reset the password, " +
                                  $"you can use the Forgot Password link on the Login page";

                    var message = new Message(new List<string> { userForAuthentication.Email }, "Locked out account information", content, null);

                    await _emailSender.SendEmailAsync(message);

                    return Unauthorized(new AuthResponseDto { ErrorMessage = "The account is locked out" });
                }


                return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid Authentication" });
            }
            if (await _userManager.GetTwoFactorEnabledAsync(user))
                return await GenerateOTPFor2Factor(user);

            // 3. If authentication is successful, create a JWT token
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtHandler.CreateToken(user, roles);

            // 4. Return an Ok response with the success status and the token
            //  مهمة في حال جبت المحاولة الاولى غلط وبعديت جبت صح مفروض يصفر العداد 
            await _userManager.ResetAccessFailedCountAsync(user);
            return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token });
        }



        private async Task<IActionResult> GenerateOTPFor2Factor(User user)
        {
            var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);
            if (!providers.Contains("Email"))
                return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid 2-Factor Provider." });

            var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

            var message = new Message(new List<string> { user.Email }, "Authentication token", token, null);

            await _emailSender.SendEmailAsync(message);

            return Ok(new AuthResponseDto { Is2FactorRequired = true, Provider = "Email" });
        }
        [HttpPost("twofactor")]
        public async Task<IActionResult> TwoFactor([FromBody] TwoFactorDto twoFactorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByEmailAsync(twoFactorDto.Email!);
            if (user is null)
            {
                return BadRequest("Invalid Request");
            }

            var validVerification = await _userManager.VerifyTwoFactorTokenAsync(user, twoFactorDto.Provider!, twoFactorDto.Token!);

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtHandler.CreateToken(user, roles);

            return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token });
        }

      

        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByEmailAsync(forgotPassword.Email!);
            if (user is null)
            {
                return BadRequest("Invalid Request");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string?>
    {
        {"token", token },
        {"email", forgotPassword.Email!}
    };

            var callback = QueryHelpers.AddQueryString(forgotPassword.ClientUri!, param);
            var message = new Message(new List<string> { user.Email }, "Reset password token", callback, null);

            await _emailSender.SendEmailAsync(message);

            return Ok();
        }

        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByEmailAsync(resetPassword.Email!);
            if (user is null)
            {
                return BadRequest("Invalid Request");
            }

            var result = await _userManager.ResetPasswordAsync(user, resetPassword.Token!, resetPassword.Password!);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);

                return BadRequest(new { Errors = errors });
            }
            await _userManager.SetLockoutEndDateAsync(user, null);
            return Ok();
        }

        [Authorize(Roles = "Employee,Manager")]
        [HttpPost("complete-profile")]
        public async Task<IActionResult> CompleteProfile([FromBody] CompleteProfileDto dto)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity!.Name!);
            if (user is null)
                return Unauthorized();


            // Update values

            user.TeamId = dto.TeamId;
            user.ManagerId = dto.ManagerId;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            return Ok("Profile completed successfully.");
        }

    }
}
