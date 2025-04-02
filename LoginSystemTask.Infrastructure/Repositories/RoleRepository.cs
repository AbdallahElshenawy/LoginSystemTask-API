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
    public class RoleRepository(ApplicationDbContext context) : BaseRepository<Role>(context), IRoleRepository
    {
        public async Task<Role?> GetByIdAsync(int id)
        {
            return await context.Roles.FirstOrDefaultAsync(r => r.Id==id);
        }

        public async Task<Role?> GetByNameAsync(string name)
        {
            return await context.Roles.FirstOrDefaultAsync(r => r.Name == name);
        }

        public async Task<Role?> GetRoleByUserIdAsync(int userId)
        {
            return await context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.Role)
                .FirstOrDefaultAsync();
        }
    }
}
