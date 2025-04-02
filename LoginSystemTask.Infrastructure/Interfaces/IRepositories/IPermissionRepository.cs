using LoginSystemTask.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystemTask.Infrastructure.Interfaces.IRepositories
{
    public interface IPermissionRepository : IBaseRepository<Permission>
    {
        Task<IEnumerable<Permission>> GetByRoleIdAsync(int roleId);

        Task<IEnumerable<Permission>> GetByUserIdAsync(int userId);
    }
}
