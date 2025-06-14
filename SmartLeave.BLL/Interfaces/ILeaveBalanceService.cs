using SmartLeave.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLeave.BLL.Interfaces
{
        public interface ILeaveBalanceService
        {
            Task InitializeForUserAsync(User user);
        }
    
}
