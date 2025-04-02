using LoginSystemTask.Infrastructure.Models;

namespace LoginSystemTask.Infrastructure.Interfaces.IRepositories
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Task<Role?> GetByNameAsync(string name);

        Task<Role?> GetRoleByUserIdAsync(int userId);
        new Task<Role?> GetByIdAsync(int id);

    }
}