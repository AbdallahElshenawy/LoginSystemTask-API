using LoginSystemTask.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystemTask.Application.Interfaces.IServices.Authentication
{
    public interface IPermissionService
    {
        Task<Permission> CreatePermissionAsync(string name);
        Task<Permission> GetPermissionByIdAsync(int id);
        Task<IEnumerable<Permission>> GetAllPermissionsAsync();
        Task UpdatePermissionAsync(int id, string name);
        Task DeletePermissionAsync(int id);
    }
}
