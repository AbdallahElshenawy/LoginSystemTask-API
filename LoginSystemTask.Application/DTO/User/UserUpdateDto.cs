using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystemTask.Application.DTO.User
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; } 
    }
}
