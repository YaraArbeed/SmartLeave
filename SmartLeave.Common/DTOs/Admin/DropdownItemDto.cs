using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLeave.Common.DTOs.Admin
{
    public class DropdownItemDto
    {
        public string Id { get; set; } = string.Empty;  // Use string for GUID or int compatibility
        public string Name { get; set; } = string.Empty;
    }
}
