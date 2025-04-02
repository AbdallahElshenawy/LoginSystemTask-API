using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystemTask.Application.DTO
{
    public class RoleWithPermissionsDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public List<string> Permissions { get; set; }
    }
}
