using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLeave.DAL.Entities
{
    public class Country
    {
        /// <summary>
        /// Primary key for Country (int).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// e.g. "Saudi Arabia", "United Arab Emirates", etc.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// All offices located in this country.
        /// </summary>
        public  ICollection<Office> Offices { get; set; }

        /// <summary>
        /// All users whose CountryId = this.Id.
        /// </summary>
        public virtual ICollection<User> Users { get; set; }
    }
}
