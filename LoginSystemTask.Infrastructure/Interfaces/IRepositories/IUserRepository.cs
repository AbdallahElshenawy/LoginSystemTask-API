using LoginSystemTask.Infrastructure.Models;
namespace LoginSystemTask.Infrastructure.Interfaces.IRepositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);
        new Task<User?> GetByIdAsync(int id); 
        new Task<bool> DeleteAsync(int id);
    }
}
