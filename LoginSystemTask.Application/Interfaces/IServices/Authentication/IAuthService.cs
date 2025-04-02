using LoginSystemTask.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystemTask.Application.Interfaces.IServices.Authentication
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginDto dto);
        Task AssignRoleToUserAsync(int userId, string roleName);

        Task UpdateUserRoleAsync(int userId, string roleName);
    }
}
