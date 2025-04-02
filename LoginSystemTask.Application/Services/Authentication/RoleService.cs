using LoginSystemTask.Application.DTO;
using LoginSystemTask.Application.Interfaces.IServices;
using LoginSystemTask.Application.Interfaces.IServices.Authentication;
using LoginSystemTask.Infrastructure.Data;
using LoginSystemTask.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginSystemTask.Application.Services.Authentication
{
    public class RoleService(ApplicationDbContext context, IAuditLogService auditLogService) : IRoleService
    {
        public async Task AddRoleAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Role name cannot be empty");

            if (await context.Roles.AnyAsync(r => r.Name == roleName))
                throw new Exception("Role already exists");

            var role = new Role { Name = roleName };
            context.Roles.Add(role);
            await context.SaveChangesAsync(); 
            await auditLogService.LogAsync("Create", "Role", role.Id.ToString(), $"Name: {roleName}");
        }

    
        public async Task UpdateRoleAsync(int id, string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Role name cannot be empty");

            var role = await context.Roles.FindAsync(id)
                ?? throw new Exception("Role not found");

            if (await context.Roles.AnyAsync(r => r.Name == roleName && r.Id != id))
                throw new Exception("Role name already exists");

            role.Name = roleName;
            await context.SaveChangesAsync();
            await auditLogService.LogAsync("Update", "Role", id.ToString(), $"Name: {roleName}");
        }

        public async Task DeleteRoleAsync(int id)
        {
            var role = await context.Roles
                .Include(r => r.Users)
                .FirstOrDefaultAsync(r => r.Id == id)
                ?? throw new Exception("Role not found");

            if (role.Users.Any())
                throw new InvalidOperationException("Cannot delete role with assigned users");

            var rolePermissions = context.RolePermissions.Where(rp => rp.RoleId == id);
            context.RolePermissions.RemoveRange(rolePermissions);

            context.Roles.Remove(role);
            await context.SaveChangesAsync();
            await auditLogService.LogAsync("Delete", "Role", id.ToString(), $"Name: {role.Name}");
        }

        public async Task AddPermissionToRoleAsync(int roleId, int permissionId)
        {
            if (!await context.Roles.AnyAsync(r => r.Id == roleId))
                throw new Exception("Role not found");
            if (!await context.Permissions.AnyAsync(p => p.Id == permissionId))
                throw new Exception("Permission not found");
            if (await context.RolePermissions.AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId))
                return;

            var rolePermission = new RolePermission { RoleId = roleId, PermissionId = permissionId };
            context.RolePermissions.Add(rolePermission);
            await context.SaveChangesAsync();
            var role = await context.Roles.FindAsync(roleId);
            var permission = await context.Permissions.FindAsync(permissionId);
            await auditLogService.LogAsync("Assign", "RolePermission", $"{roleId}-{permissionId}",
                $"Role: {role.Name}, Permission: {permission.Name}");
        }

        public async Task RemovePermissionFromRoleAsync(int roleId, int permissionId)
        {
            var rp = await context.RolePermissions
                .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
            if (rp == null) return;

            context.RolePermissions.Remove(rp);
            await context.SaveChangesAsync();
            var role = await context.Roles.FindAsync(roleId);
            var permission = await context.Permissions.FindAsync(permissionId);
            await auditLogService.LogAsync("Remove", "RolePermission", $"{roleId}-{permissionId}",
                $"Role: {role.Name}, Permission: {permission.Name}");
        }

        public async Task<IEnumerable<string>> GetPermissionsForRoleAsync(int roleId)
        {
            var permissions = await context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Join(context.Permissions, rp => rp.PermissionId, p => p.Id, (rp, p) => p.Name)
                .ToListAsync();
            return permissions;
        }

        public async Task<IEnumerable<RoleWithPermissionsDto>> GetAllRolesWithPermissionsAsync()
        {
            var roles = await context.Roles
                .Select(r => new RoleWithPermissionsDto
                {
                    RoleId = r.Id,
                    RoleName = r.Name,
                    Permissions = r.RolePermissions.Select(rp => rp.Permission.Name).ToList()
                })
                .ToListAsync();
            return roles;
        }
    }
}