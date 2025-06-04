using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLeave.DAL.Entities
{
    public class Office
    {
        /// <summary>
        /// Primary key for Office (int).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// e.g. "Riyadh HQ", "Jeddah Branch", etc.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// FK → Country.Id (the country where this office is located).
        /// </summary>
        public int CountryId { get; set; }

        /// <summary>
        /// Navigation to the Country entity.
        /// </summary>
        public  Country Country { get; set; }

        /// <summary>
        /// All users whose User.OfficeId = this.Id (employees assigned to this office).
        /// </summary>
        public virtual ICollection<User> Users { get; set; }
    }
}
