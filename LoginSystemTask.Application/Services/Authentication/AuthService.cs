using LoginSystemTask.Application.DTO;
using LoginSystemTask.Application.Interfaces.IServices.Authentication;
using LoginSystemTask.Infrastructure.Data;
using LoginSystemTask.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystemTask.Application.Services.Authentication
{
    public class AuthService(ApplicationDbContext context,IConfiguration configuration) : IAuthService
    {
        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid username or password");

            return GenerateJwtToken(user);
        }
        public async Task AssignRoleToUserAsync(int userId, string roleName)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new Exception("User not found");

            var role = await context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null) throw new Exception("Role not found");

            user.RoleId = role.Id;
            await context.SaveChangesAsync();
        }
        public async Task UpdateUserRoleAsync(int userId, string roleName)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new Exception("User not found");

            var role = await context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null) throw new Exception("Role not found");

            user.RoleId = role.Id;
            await context.SaveChangesAsync();
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}