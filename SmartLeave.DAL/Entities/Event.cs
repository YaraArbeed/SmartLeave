using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLeave.DAL.Entities
{
    public class Event
    {
        /// <summary>
        /// Primary key for Event (int).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// e.g. "Company Townhall", "Quarterly Offsite".
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Date of the event.
        /// </summary>
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Optional description or details about the event.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The user who created this event—FK to AspNetUsers(Id).
        /// </summary>
        public string CreatedById { get; set; }

        /// <summary>
        /// Navigation to the user who created the event.
        /// </summary>
        public virtual User CreatedBy { get; set; }

        /// <summary>
        /// When the event was created.
        /// </summary>
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
