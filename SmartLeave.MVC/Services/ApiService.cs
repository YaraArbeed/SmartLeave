using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartLeave.Common.DTOs.Admin;
using SmartLeave.Common.DTOs.All;
using SmartLeave.MVC.Models;
using SmartLeave.MVC.Services;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace SmartLeave.MVC.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        private void SetAuthorizationHeader()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWTToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<LeaveResponseModel>> GetMyLeavesAsync()
        {
            SetAuthorizationHeader();

            var response = await _httpClient.GetAsync("api/Leaves/mine");

            // Add debugging
            var responseContent = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"Status: {response.StatusCode}");
            System.Diagnostics.Debug.WriteLine($"Content: {responseContent}");

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<List<LeaveResponseModel>>(responseContent, options) ?? new List<LeaveResponseModel>();
            }

            // Log the error response
            System.Diagnostics.Debug.WriteLine($"API Error: {response.StatusCode} - {responseContent}");
            return new List<LeaveResponseModel>();
        }

        public async Task<List<LeaveBalanceModel>> GetMyLeaveBalancesAsync()
        {
            SetAuthorizationHeader();

            var response = await _httpClient.GetAsync("api/Leaves/balance");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<List<LeaveBalanceModel>>(json, options) ?? new List<LeaveBalanceModel>();
            }

            return new List<LeaveBalanceModel>();
        }

        public async Task<bool> RequestLeaveAsync(LeaveRequestModel request)
        {
            SetAuthorizationHeader();

            using var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(request.LeaveTypeId.ToString()), "LeaveTypeId");
            formData.Add(new StringContent(request.StartDate.ToString("yyyy-MM-dd")), "StartDate");
            formData.Add(new StringContent(request.EndDate.ToString("yyyy-MM-dd")), "EndDate");

            if (!string.IsNullOrEmpty(request.Comment))
                formData.Add(new StringContent(request.Comment), "Comment");

            if (request.Attachment != null)
                formData.Add(new StreamContent(request.Attachment.OpenReadStream()), "Attachment", request.Attachment.FileName);

            var response = await _httpClient.PostAsync("api/Leaves/request", formData);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<PublicHolidayModel>> GetPublicHolidaysAsync(string country)
        {
            var response = await _httpClient.GetAsync($"api/PublicHolidays/{country}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<List<PublicHolidayModel>>(json, options) ?? new List<PublicHolidayModel>();
            }

            return new List<PublicHolidayModel>();
        }

        public async Task<List<LeaveTypeModel>> GetLeaveTypesAsync()
        {
            SetAuthorizationHeader(); // If protected

            var response = await _httpClient.GetAsync("api/LeaveTypes");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<List<LeaveTypeModel>>(json, options) ?? new List<LeaveTypeModel>();
            }

            return new List<LeaveTypeModel>();
        }
        public async Task<List<PendingLeaveModel>> GetPendingLeavesAsync()
        {
            SetAuthorizationHeader();

            var response = await _httpClient.GetAsync("api/manager/leaves/pending");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<List<PendingLeaveModel>>(json, options) ?? new List<PendingLeaveModel>();
            }

            // Log the error response
            var errorContent = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"API Error: {response.StatusCode} - {errorContent}");
            return new List<PendingLeaveModel>();
        }

        public async Task<bool> ProcessLeaveDecisionAsync(LeaveDecisionModel decision)
        {
            SetAuthorizationHeader();

            var json = JsonSerializer.Serialize(decision);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/manager/leaves/decision", content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            // Log the error response
            var errorContent = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"Decision API Error: {response.StatusCode} - {errorContent}");
            return false;
        }

        public async Task<PendingLeaveModel> GetLeaveDetailsAsync(int leaveId)
        {
            SetAuthorizationHeader();

            var response = await _httpClient.GetAsync($"api/manager/leaves/{leaveId}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<PendingLeaveModel>(json, options) ?? new PendingLeaveModel();
            }

            return new PendingLeaveModel();
        }
        //Admin
        public async Task<List<AdminLeaveModel>> GetAdminLeavesAsync(AdminFilterModel filter)
        {
            SetAuthorizationHeader();

            try
            {
                // Map MVC filter model to API DTO
                var requestBody = new
                {
                    fromDate = filter.StartDate,
                    toDate = filter.EndDate,
                    teamId = filter.TeamId,
                    leaveTypeId = filter.LeaveTypeId,
                    officeId = filter.OfficeId,
                    countryId = filter.CountryId,
                    status = filter.Status,
                    approverId = filter.ApproverId,
                    employeeId = filter.EmployeeId
                };

                var json = JsonSerializer.Serialize(requestBody);
                Console.WriteLine($"Leave Request Body: {json}");

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/admin/reports/leaves", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Leaves API Response: {responseJson}");

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var leaves = JsonSerializer.Deserialize<List<AdminLeaveModel>>(responseJson, options);
                    return leaves ?? new List<AdminLeaveModel>();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Leaves API Error: {response.StatusCode} - {errorContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetAdminLeavesAsync: {ex.Message}");
            }

            return new List<AdminLeaveModel>();
        }

        public async Task<FilterOptionsModel> GetAdminFilterOptionsAsync()
        {
            SetAuthorizationHeader();
            var filterOptions = new FilterOptionsModel();

            try
            {
                // Get employees
                var employeesResponse = await _httpClient.GetAsync("api/Employees");
                if (employeesResponse.IsSuccessStatusCode)
                {
                    var json = await employeesResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Employees API Response: {json}");

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var employees = JsonSerializer.Deserialize<List<DropdownItemDto>>(json, options);

                    if (employees != null)
                    {
                        filterOptions.Employees = employees.Select(e =>
                            new SelectListItem { Value = e.Id, Text = e.Name }).ToList();
                    }
                }

                // Get teams
                var teamsResponse = await _httpClient.GetAsync("api/Teams");
                if (teamsResponse.IsSuccessStatusCode)
                {
                    var json = await teamsResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Teams API Response: {json}");

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var teams = JsonSerializer.Deserialize<List<DropdownItemDto>>(json, options);

                    if (teams != null)
                    {
                        filterOptions.Teams = teams.Select(t =>
                            new SelectListItem { Value = t.Id, Text = t.Name }).ToList();
                    }
                }

                // Get leave types
                var leaveTypesResponse = await _httpClient.GetAsync("api/LeaveTypes");
                if (leaveTypesResponse.IsSuccessStatusCode)
                {
                    var json = await leaveTypesResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"LeaveTypes API Response: {json}");

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var leaveTypes = JsonSerializer.Deserialize<List<LeaveTypeDto>>(json, options);

                    if (leaveTypes != null)
                    {
                        filterOptions.LeaveTypes = leaveTypes.Select(lt =>
                            new SelectListItem { Value = lt.Id.ToString(), Text = lt.Name }).ToList();
                    }
                }

                // Get offices
                var officesResponse = await _httpClient.GetAsync("api/Offices");
                if (officesResponse.IsSuccessStatusCode)
                {
                    var json = await officesResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Offices API Response: {json}");

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var offices = JsonSerializer.Deserialize<List<DropdownItemDto>>(json, options);

                    if (offices != null)
                    {
                        filterOptions.Offices = offices.Select(o =>
                            new SelectListItem { Value = o.Id, Text = o.Name }).ToList();
                    }
                }

                // Get countries
                var countriesResponse = await _httpClient.GetAsync("api/Countries");
                if (countriesResponse.IsSuccessStatusCode)
                {
                    var json = await countriesResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Countries API Response: {json}");

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var countries = JsonSerializer.Deserialize<List<DropdownItemDto>>(json, options);

                    if (countries != null)
                    {
                        filterOptions.Countries = countries.Select(c =>
                            new SelectListItem { Value = c.Id, Text = c.Name }).ToList();
                    }
                }

                // Get approvers
                var approversResponse = await _httpClient.GetAsync("api/Approvers");
                if (approversResponse.IsSuccessStatusCode)
                {
                    var json = await approversResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Approvers API Response: {json}");

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var approvers = JsonSerializer.Deserialize<List<DropdownItemDto>>(json, options);

                    if (approvers != null)
                    {
                        filterOptions.Approvers = approvers.Select(a =>
                            new SelectListItem { Value = a.Id, Text = a.Name }).ToList();
                    }
                }

                // Get statuses
                var statusesResponse = await _httpClient.GetAsync("api/Statuses");
                if (statusesResponse.IsSuccessStatusCode)
                {
                    var json = await statusesResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Statuses API Response: {json}");

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var statuses = JsonSerializer.Deserialize<List<DropdownItemDto>>(json, options);

                    if (statuses != null)
                    {
                        filterOptions.Statuses = statuses.Select(s =>
                            new SelectListItem { Value = s.Id, Text = s.Name }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetAdminFilterOptionsAsync: {ex.Message}");
            }

            return filterOptions;
        }

        public async Task<bool> ProcessAdminLeaveDecisionAsync(LeaveDecisionModel decision)
        {
            SetAuthorizationHeader();

            try
            {
                var requestBody = new
                {
                    leaveId = decision.LeaveId,
                    isApproved = decision.IsApproved,
                    note = decision.Note
                };

                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/admin/reports/decision", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Decision API Error: {response.StatusCode} - {errorContent}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in ProcessAdminLeaveDecisionAsync: {ex.Message}");
            }

            return false;
        }
    }
}

