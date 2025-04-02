using LoginSystemTask.Application.DTO.User;
using LoginSystemTask.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystemTask.Application.Interfaces.IServices
{
    public interface IUserService
    {
        Task<UserResponseDto> AddUserAsync(UserAddDto dto);
        Task<UserResponseDto> UpdateUserAsync(int id, UserUpdateDto dto);
        Task DeleteUserAsync(int id); 
        Task<UserResponseDto?> GetUserByIdAsync(int id);
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
        Task<UserResponseDto?> GetUserByUsernameAsync(string username);
    }
}
