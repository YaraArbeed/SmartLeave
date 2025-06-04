using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLeave.DAL.Entities
{
    public class Team
    {
        /// <summary>
        /// Primary key (int).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// e.g. "Engineering East", "Sales Team A", etc.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The UserId (string) of the manager who leads this team.
        /// This must correspond to User.Id in AspNetUsers.
        /// </summary>
        public string ManagerId { get; set; }

        /// <summary>
        /// Navigation to the User (Manager) who leads this team.
        /// </summary>
        public virtual User Manager { get; set; }

        /// <summary>
        /// All users whose User.TeamId = this.Id.
        /// all the users in this team
        /// </summary>
        public virtual ICollection<User> Members { get; set; }
    }
}
