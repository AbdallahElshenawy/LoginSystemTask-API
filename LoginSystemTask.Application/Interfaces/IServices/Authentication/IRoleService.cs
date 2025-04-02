using LoginSystemTask.Application.DTO;
using LoginSystemTask.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystemTask.Application.Interfaces.IServices.Authentication
{
    public interface IRoleService
    {
        Task AddRoleAsync(string roleName);

        Task UpdateRoleAsync(int id, string roleName); 
        Task DeleteRoleAsync(int id);
        Task AddPermissionToRoleAsync(int roleId, int permissionId);
        Task RemovePermissionFromRoleAsync(int roleId, int permissionId);
        Task<IEnumerable<string>> GetPermissionsForRoleAsync(int roleId);
        Task<IEnumerable<RoleWithPermissionsDto>> GetAllRolesWithPermissionsAsync();


     
    }
}
