using LoginSystemTask.Infrastructure.Data;
using LoginSystemTask.Infrastructure.Interfaces.IRepositories;
using LoginSystemTask.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LoginSystemTask.Infrastructure.Repositories
{
    public class UserRepository(ApplicationDbContext context) : BaseRepository<User>(context), IUserRepository
    {
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public new async Task<User?> GetByIdAsync(int id)
        {
            return await context.Users
                .Include(u => u.Role) 
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public  async Task AddAsync(User entity)
        {
            await context.Users.AddAsync(entity);
        }

        public  async Task<bool> UpdateAsync(User entity)
        {
            context.Users.Update(entity);
            return await context.SaveChangesAsync() > 0;
        }

        public new async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return false;

            context.Users.Remove(entity);
            return await context.SaveChangesAsync() > 0;
        }
    }
}