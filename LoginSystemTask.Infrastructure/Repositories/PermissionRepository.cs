using LoginSystemTask.Infrastructure.Data;
using LoginSystemTask.Infrastructure.Interfaces.IRepositories;
using LoginSystemTask.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystemTask.Infrastructure.Repositories
{
    public class PermissionRepository(ApplicationDbContext context) : BaseRepository<Permission>(context), IPermissionRepository
    {
        public async Task<IEnumerable<Permission>> GetByRoleIdAsync(int roleId)
        {
            return await context.RolePermissions
         .Where(rp => rp.RoleId == roleId)
         .Join(context.Permissions,
               rp => rp.PermissionId,
               p => p.Id,
               (rp, p) => p)
         .ToListAsync();
        }

        public async Task<IEnumerable<Permission>> GetByUserIdAsync(int userId)
        {
            return await context.Users
                .Where(u => u.Id == userId)
                .Join(context.RolePermissions,
                      u => u.RoleId,
                      rp => rp.RoleId,
                      (u, rp) => rp.PermissionId)
                .Join(context.Permissions,
                      pid => pid,
                      p => p.Id,
                      (pid, p) => p)
                .Distinct()
                .ToListAsync();
        }
    }
}