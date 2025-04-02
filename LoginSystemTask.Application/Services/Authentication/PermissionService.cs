using LoginSystemTask.Application.Interfaces.IServices;
using LoginSystemTask.Application.Interfaces.IServices.Authentication;
using LoginSystemTask.Infrastructure.Data;
using LoginSystemTask.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginSystemTask.Application.Services.Authentication
{
    public class PermissionService(ApplicationDbContext context, IAuditLogService auditLogService) : IPermissionService
    {
        public async Task<Permission> CreatePermissionAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Permission name cannot be empty");
            if (await context.Permissions.AnyAsync(p => p.Name == name))
                throw new Exception("Permission name already exists");

            var permission = new Permission { Name = name };
            context.Permissions.Add(permission);
            await context.SaveChangesAsync();
            await auditLogService.LogAsync("Create", "Permission", permission.Id.ToString(), $"Name: {name}");

            return permission;
        }

        public async Task<Permission> GetPermissionByIdAsync(int id)
        {
            var permission = await context.Permissions.FindAsync(id)
                ?? throw new Exception("Permission not found");
            return permission; 
        }

        public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
        {
            return await context.Permissions.ToListAsync(); 
        }

        public async Task UpdatePermissionAsync(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Permission name cannot be empty");

            var permission = await context.Permissions.FindAsync(id)
                ?? throw new Exception("Permission not found");
            if (await context.Permissions.AnyAsync(p => p.Name == name && p.Id != id))
                throw new Exception("Permission name already exists");

            permission.Name = name;
            await context.SaveChangesAsync();
            await auditLogService.LogAsync("Update", "Permission", id.ToString(), $"Name: {name}");
        }

        public async Task DeletePermissionAsync(int id)
        {
            var permission = await context.Permissions.FindAsync(id)
                ?? throw new Exception("Permission not found");

            var rolePermissions = context.RolePermissions.Where(rp => rp.PermissionId == id);
            context.RolePermissions.RemoveRange(rolePermissions);
            context.Permissions.Remove(permission);
            await context.SaveChangesAsync();
            await auditLogService.LogAsync("Delete", "Permission", id.ToString(), $"Name: {permission.Name}");
        }
    }
}