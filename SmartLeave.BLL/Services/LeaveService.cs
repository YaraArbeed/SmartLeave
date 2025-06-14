using EmailService;
using SmartLeave.BLL.Interfaces;
using SmartLeave.Common.DTOs.Admin;
using SmartLeave.Common.DTOs.Employee;
using SmartLeave.Common.DTOs.Manager;
using SmartLeave.DAL.Entities;
using SmartLeave.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLeave.BLL.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly ILeaveRepository _leaveRepo;
        private readonly IUserRepository _userRepo;
        private readonly IEmailSender _emailSender;
        public LeaveService(ILeaveRepository leaveRepo, IUserRepository userRepo, IEmailSender emailSender)
        {
            _leaveRepo = leaveRepo;
            _userRepo = userRepo;
            _emailSender = emailSender;

        }

        public async Task<bool> ApplyForLeaveAsync(string userEmail, LeaveRequestDto dto)
        {
            var employee = await _userRepo.GetUserByEmailAsync(userEmail);
            if (employee == null) return false;

            var overlap = await _leaveRepo.HasOverlappingLeave(employee.Id, dto.StartDate, dto.EndDate);
            if (overlap) return false;

            var days = (decimal)(dto.EndDate - dto.StartDate).TotalDays + 1;
            var balance = await _leaveRepo.GetLeaveBalanceAsync(employee.Id, dto.LeaveTypeId);

            if (balance.RemainingDays < days)
                return false;

            string? savedPath = null;
            if (dto.Attachment != null)
            {
                var uploadsFolder = Path.Combine("wwwroot", "attachments");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}_{dto.Attachment.FileName}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Attachment.CopyToAsync(stream);
                }

                savedPath = $"/attachments/{fileName}"; // URL-friendly path
            }

            var leave = new Leave
            {
                EmployeeId = employee.Id,
                LeaveTypeId = dto.LeaveTypeId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                TotalDays = days,
                Status = LeaveStatus.Pending,
                AppliedOn = DateTime.Now,
                Comment = dto.Comment,
                AttachmentUrl = savedPath,
                ApprovalNote = dto.ApprovalNote,
                ApprovedById = dto.ApprovedById
            };

            return await _leaveRepo.AddLeaveAsync(leave);
        }

        public async Task<List<LeaveResponseDto>> GetMyLeavesAsync(string userEmail)
        {
            var employee = await _userRepo.GetUserByEmailAsync(userEmail);
            var leaves = await _leaveRepo.GetLeavesByEmployeeIdAsync(employee.Id);
            return leaves.Select(l => new LeaveResponseDto
            {
                Id = l.Id,
                LeaveType = l.LeaveType.Name,
                StartDate = l.StartDate,
                EndDate = l.EndDate,
                Status = l.Status.ToString(),
                ManagerName = l.ApprovedBy?.UserName ?? "-",
                Comment = l.Comment,
                TotalDays = l.TotalDays,
                AppliedBy = l.Employee.Email,
                AppliedOn = l.AppliedOn,
                AttachmentUrl = l.AttachmentUrl
            }).ToList();
        }

        public async Task<List<LeaveBalanceDto>> GetMyLeaveBalancesAsync(string userEmail)
        {
            var employee = await _userRepo.GetUserByEmailAsync(userEmail);
            var balances = await _leaveRepo.GetLeaveBalancesAsync(employee.Id);
            return balances.Select(b => new LeaveBalanceDto
            {
                LeaveType = b.LeaveType.Name,
                UsedDays = b.UsedDays,
                RemainingDays = b.RemainingDays,
                Allowance = b.LeaveType.AnnualQuota
            }).ToList();
        }

        //-------------------------------------Manager---------------------------------------------------
        public async Task<List<LeaveResponseDto>> GetPendingForManagerAsync(string mgrEmail)
        {
            var manager = await _userRepo.GetUserByEmailAsync(mgrEmail);
            var teamId = manager.TeamId ?? 0;                        
            var leaves = await _leaveRepo.GetPendingByTeamAsync(teamId);

            return leaves.Select(l => new LeaveResponseDto
            {
                Id = l.Id,
                LeaveType = l.LeaveType.Name,
                StartDate = l.StartDate,
                EndDate = l.EndDate,
                Status = l.Status.ToString(),
                ManagerName = manager?.UserName ?? "-",
                Comment = l.Comment ?? "",
                TotalDays = l.TotalDays,
                AppliedBy = l.Employee.Email,
                AppliedOn = l.AppliedOn, 
                AttachmentUrl = l.AttachmentUrl 
            }).ToList();
        }

        public async Task<bool> DecideAsync(string mgrEmail, LeaveDecisionDto dto)
        {
            var manager = await _userRepo.GetUserByEmailAsync(mgrEmail);
            var leave = await _leaveRepo.GetByIdAsync(dto.LeaveId);
            if (leave == null || leave.Status != LeaveStatus.Pending) return false;

            // security – only manager of that employee’s team
            if (leave.Employee.ManagerId != manager.Id) return false;

            // revert balance *only* on rejection
            if (!dto.IsApproved)
                await _leaveRepo.RestoreBalanceAsync(leave.EmployeeId, leave.LeaveTypeId, leave.TotalDays);

            leave.Status = dto.IsApproved ? LeaveStatus.Approved : LeaveStatus.Rejected;
            leave.ApprovedById = manager.Id;
            leave.DecisionDate = DateTime.UtcNow;
            leave.ApprovalNote = dto.Note;

            var result = await _leaveRepo.UpdateAsync(leave);

            if (result)
            {
                var subject = $"Your Leave Request has been {(dto.IsApproved ? "Approved" : "Rejected")}";
                var body = $"Dear {leave.Employee.UserName},<br/>" +
                           $"Your leave from {leave.StartDate:yyyy-MM-dd} to {leave.EndDate:yyyy-MM-dd} " +
                           $"has been <b>{leave.Status}</b>.<br/>" +
                           $"Manager's Note: {leave.ApprovalNote ?? "None"}";

                var message = new Message(new List<string> { leave.Employee.Email }, subject, body, null);

                await _emailSender.SendEmailAsync(message);
            }

            return result;
        }
        public async Task<List<LeaveResponseDto>> GetReportAsync(LeaveReportFilterDto filter)
        {
            // fetch entity list
            var leaves = await _leaveRepo.QueryLeavesAsync(filter);

            // map to DTOs
            return leaves.Select(l => new LeaveResponseDto
            {
                Id = l.Id,
                LeaveType = l.LeaveType.Name,
                StartDate = l.StartDate,
                EndDate = l.EndDate,
                TotalDays = l.TotalDays,
                Status = l.Status.ToString(),
                ManagerName = l.Employee.Manager?.UserName ?? "-",
                Comment = l.Comment,
                AppliedBy = l.Employee.Email,
                AppliedOn = l.AppliedOn,
                AttachmentUrl = l.AttachmentUrl
            }).ToList();
        }

        public async Task<bool> AdminDecideAsync(string adminEmail, LeaveDecisionDto dto)
        {
            // get admin user
            var admin = await _userRepo.GetUserByEmailAsync(adminEmail);

            // get target leave
            var leave = await _leaveRepo.GetByIdAsync(dto.LeaveId);
            if (leave == null || leave.Status != LeaveStatus.Pending) return false;

            // no team restriction for admin
            if (!dto.IsApproved)
                await _leaveRepo.RestoreBalanceAsync(leave.EmployeeId, leave.LeaveTypeId, leave.TotalDays);

            // update fields
            leave.Status = dto.IsApproved ? LeaveStatus.Approved : LeaveStatus.Rejected;
            leave.ApprovedById = admin.Id;
            leave.DecisionDate = DateTime.UtcNow;
            leave.ApprovalNote = dto.Note;

            // persist
            var ok = await _leaveRepo.UpdateAsync(leave);
            if (ok)
            {
                var subject = $"Your leave was {(dto.IsApproved ? "Approved" : "Rejected")} by Admin";
                var body = $"Hello {leave.Employee.UserName},<br/>" +
                              $"Your leave {leave.StartDate:yyyy-MM-dd}→{leave.EndDate:yyyy-MM-dd} " +
                              $"has been <b>{leave.Status}</b>.<br/>Note: {leave.ApprovalNote ?? "None"}";
                await _emailSender.SendEmailAsync(
                  new Message(new[] { leave.Employee.Email }, subject, body, null));
            }
            return ok;
        }

    }
}