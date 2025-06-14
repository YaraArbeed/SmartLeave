using Microsoft.EntityFrameworkCore;
using SmartLeave.BLL.Interfaces;
using SmartLeave.DAL.Data;
using SmartLeave.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLeave.BLL.Services
{
    public class LeaveBalanceService : ILeaveBalanceService
    {
        private readonly ApplicationDbContext _context;

        public LeaveBalanceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task InitializeForUserAsync(User user)
        {
            var currentYear = (short)DateTime.Now.Year;

            var leaveTypes = await _context.LeaveTypes.ToListAsync();
            foreach (var type in leaveTypes)
            {
                _context.LeaveBalances.Add(new LeaveBalance
                {
                    EmployeeId = user.Id,
                    LeaveTypeId = type.Id,
                    Year = currentYear,
                    UsedDays = 0,
                    RemainingDays = type.AnnualQuota
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}

