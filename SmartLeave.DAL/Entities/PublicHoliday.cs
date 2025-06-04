using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLeave.DAL.Entities
{
    public class PublicHoliday
    {
        /// <summary>
        /// Primary key for PublicHoliday (int).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// e.g. "National Day", "Students’ Day", etc.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The date of the holiday (must be unique).
        /// </summary>
        public DateTime HolidayDate { get; set; }

        /// <summary>
        /// Optional description or notes.
        /// </summary>
        public string Description { get; set; }
    }
}

