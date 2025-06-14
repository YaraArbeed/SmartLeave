using Microsoft.EntityFrameworkCore;
using SmartLeave.Common.DTOs.Admin;
using SmartLeave.DAL.Data;
using SmartLeave.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLeave.DAL.Repositories
{
    public class LeaveRepository : ILeaveRepository
    {
        private readonly ApplicationDbContext _context;

        public LeaveRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddLeaveAsync(Leave leave)
        {
            _context.Leaves.Add(leave);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HasOverlappingLeave(string employeeId, DateTime start, DateTime end)
        {
            return await _context.Leaves.AnyAsync(l =>
                l.EmployeeId == employeeId &&
                l.Status != LeaveStatus.Rejected &&
                l.EndDate >= start &&
                l.StartDate <= end);
        }

        public async Task<List<Leave>> GetLeavesByEmployeeIdAsync(string employeeId, int skip, int take)
        {
            return await _context.Leaves
                .Include(l => l.LeaveType)
                .Include(l => l.ApprovedBy)
                .Where(l => l.EmployeeId == employeeId)
                .OrderByDescending(l => l.AppliedOn)
                .Skip(skip)                            
                .Take(take)
                .ToListAsync();
        }

        public async Task<List<LeaveBalance>> GetLeaveBalancesAsync(string employeeId)
        {
            return await _context.LeaveBalances
                .Include(b => b.LeaveType)
                .Where(b => b.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<LeaveBalance?> GetLeaveBalanceAsync(string employeeId, int leaveTypeId)
        {
            return await _context.LeaveBalances.FirstOrDefaultAsync(b =>
                b.EmployeeId == employeeId && b.LeaveTypeId == leaveTypeId);
        }
        //-----------------------------Manager-----------------------------------------------------

        public async Task<Leave?> GetByIdAsync(int leaveId)
        {
            return await _context.Leaves                     
                .Include(l => l.Employee)                    // include employee so we know TeamId / ManagerId
                .FirstOrDefaultAsync(l => l.Id == leaveId);
        }

        public async Task<List<Leave>> GetPendingByTeamAsync(int teamId, int skip, int take)
        {
            return await _context.Leaves
                .Include(l => l.LeaveType)
                .Include(l => l.Employee)
                .Where(l => l.Status == LeaveStatus.Pending &&
                            l.Employee.TeamId == teamId)     // only employee’s team
                .OrderBy(l => l.AppliedOn)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }



        public async Task<bool> UpdateAsync(Leave leave)
        {
            _context.Leaves.Update(leave);
            var affected = await _context.SaveChangesAsync();
            return affected > 0;
        }
        public async Task<List<Leave>> QueryLeavesAsync(LeaveReportFilterDto f)
        {
            //Eager Loading
            var q = _context.Leaves
                .Include(l => l.Employee).ThenInclude(u => u.Team)
                .Include(l => l.Employee).ThenInclude(u => u.Office)
                .Include(l => l.Employee).ThenInclude(u => u.Country)
                .Include(l => l.Employee).ThenInclude(e => e.Manager) 
                .Include(l => l.LeaveType)
                .Include(l => l.ApprovedBy)
                .AsQueryable();

            if (f.FromDate.HasValue) q = q.Where(l => l.StartDate >= f.FromDate.Value);
            if (f.ToDate.HasValue) q = q.Where(l => l.EndDate <= f.ToDate.Value);
            if (f.TeamId.HasValue) q = q.Where(l => l.Employee.TeamId == f.TeamId);
            if (f.LeaveTypeId.HasValue) q = q.Where(l => l.LeaveTypeId == f.LeaveTypeId);
            if (f.OfficeId.HasValue) q = q.Where(l => l.Employee.OfficeId == f.OfficeId);
            if (f.CountryId.HasValue) q = q.Where(l => l.Employee.CountryId == f.CountryId);
            if (!string.IsNullOrWhiteSpace(f.Status) &&
                Enum.TryParse<LeaveStatus>(f.Status, true, out var parsedStatus))
            {
                q = q.Where(l => l.Status == parsedStatus);
            }
            if (!string.IsNullOrEmpty(f.ApproverId)) q = q.Where(l => l.ApprovedById == f.ApproverId);
            if (!string.IsNullOrEmpty(f.EmployeeId)) q = q.Where(l => l.EmployeeId == f.EmployeeId);
            q = q.OrderBy(l => l.AppliedOn);
            return await q.ToListAsync();
        }


    }
}
