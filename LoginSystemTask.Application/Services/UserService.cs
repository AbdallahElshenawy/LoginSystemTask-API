using LoginSystemTask.Application.DTO.User;
using LoginSystemTask.Application.Interfaces.IServices;
using LoginSystemTask.Infrastructure.Interfaces;
using LoginSystemTask.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
namespace LoginSystemTask.Application.Services
{
    public class UserService(IUnitOfWork unitOfWork, IAuditLogService auditLogService) : IUserService
    {
        private static UserResponseDto MapToDto(User user) => new()
        {
            Id = user.Id,
            Username = user.Username,
            RoleId = user.RoleId,
            RoleName = user.Role?.Name
        };

        public async Task<UserResponseDto> AddUserAsync(UserAddDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username))
                throw new ArgumentException("Username is required.");

            if (string.IsNullOrWhiteSpace(dto.PasswordHash))
                throw new ArgumentException("PasswordHash is required.");

            var roleExists = await unitOfWork.Roles.GetByIdAsync(dto.RoleId);
            if (roleExists == null)
                throw new ArgumentException($"Role with ID {dto.RoleId} does not exist.");

            var existingUser = await unitOfWork.Users.GetByUsernameAsync(dto.Username);
            if (existingUser != null)
                throw new InvalidOperationException("Username already exists.");

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = dto.PasswordHash,
                RoleId = dto.RoleId
            };

            try
            {
                await unitOfWork.Users.AddAsync(user);
                await unitOfWork.Complete();
                await auditLogService.LogAsync("Create", "User", user.Id.ToString(), $"Username: {dto.Username}, RoleId: {dto.RoleId}");
                user.Role = await unitOfWork.Roles.GetByIdAsync(user.RoleId);
                await unitOfWork.Complete();
                return MapToDto(user);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Failed to save user.", ex);
            }
        }

        public async Task<UserResponseDto> UpdateUserAsync(int id, UserUpdateDto dto)
        {
            if (id != dto.Id)
                throw new ArgumentException("ID in URL must match ID in body.");

            var user = await unitOfWork.Users.GetByIdAsync(id) ?? throw new Exception("User not found.");

            if (string.IsNullOrWhiteSpace(dto.Username))
                throw new ArgumentException("Username is required.");

            if (string.IsNullOrWhiteSpace(dto.PasswordHash))
                throw new ArgumentException("PasswordHash is required.");

            var roleExists = await unitOfWork.Roles.GetByIdAsync(dto.RoleId);
            if (roleExists == null)
                throw new ArgumentException($"Role with ID {dto.RoleId} does not exist.");

            var existingUser = await unitOfWork.Users.GetByUsernameAsync(dto.Username);
            if (existingUser != null && existingUser.Id != id)
                throw new InvalidOperationException("Username already exists.");

            user.Username = dto.Username;
            user.PasswordHash = dto.PasswordHash;
            user.RoleId = dto.RoleId;

            try
            {
                await unitOfWork.Users.UpdateAsync(user);
                await auditLogService.LogAsync("Update", "User", user.Id.ToString(), $"Username: {dto.Username}, RoleId: {dto.RoleId}");
                await unitOfWork.Complete();
                user.Role = await unitOfWork.Roles.GetByIdAsync(user.RoleId);
                return MapToDto(user);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Failed to update user.", ex);
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await unitOfWork.Users.GetByIdAsync(id) ?? throw new Exception("User not found.");

            if (user.Employee != null)
                throw new InvalidOperationException("Cannot delete user linked to an employee.");

            try
            {
                await unitOfWork.Users.DeleteAsync(id);
                await auditLogService.LogAsync("Delete", "User", id.ToString(), $"Username: {user.Username}");
                await unitOfWork.Complete();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Failed to delete user.", ex);
            }
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(int id)
        {
            var user = await unitOfWork.Users.GetByIdAsync(id);
            return user != null ? MapToDto(user) : null; 
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await unitOfWork.Users.GetAllAsync(includes: new[] { "Role" });
            return users.Select(MapToDto); 
        }

        public async Task<UserResponseDto?> GetUserByUsernameAsync(string username)
        {
            var user = await unitOfWork.Users.GetByUsernameAsync(username);
            return user != null ? MapToDto(user) : null; 
        }
    }
}