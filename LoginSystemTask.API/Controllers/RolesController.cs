using LoginSystemTask.Application.Interfaces.IServices.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginSystemTask.API.Controllers
{
    /// <summary>
    /// Controller for managing roles and their permissions.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController(IRoleService roleService) : ControllerBase
    {
        /// <summary>
        /// Creates a new role in the system.
        /// </summary>
        /// <param name="roleName">The name of the role to create.</param>
        /// <returns>A success message if the role is created.</returns>
        /// <response code="200">Role created successfully.</response>
        /// <response code="400">Invalid input (e.g., empty role name, role already exists).</response>
        /// <response code="401">Unauthorized if the caller lacks admin privileges.</response>
        /// <response code="500">Server error if the role creation fails.</response>
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddRole([FromBody] string roleName)
        {
            try
            {
                await roleService.AddRoleAsync(roleName);
                return Ok("Role added successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Assigns a permission to a role.
        /// </summary>
        /// <param name="roleId">The ID of the role to assign the permission to.</param>
        /// <param name="permissionId">The ID of the permission to assign.</param>
        /// <returns>A success response if the permission is assigned.</returns>
        /// <response code="200">Permission assigned successfully.</response>
        /// <response code="400">Invalid input (e.g., role or permission not found).</response>
        /// <response code="401">Unauthorized if the caller lacks admin privileges.</response>
        /// <response code="500">Server error if the assignment fails.</response>
        /// <remarks>If the permission is already assigned, the request is ignored.</remarks>
        [HttpPost("{roleId}/permissions/{permissionId}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddPermission(int roleId, int permissionId)
        {
            try
            {
                await roleService.AddPermissionToRoleAsync(roleId, permissionId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all roles with their associated permissions.
        /// </summary>
        /// <returns>A list of roles with their permissions.</returns>
        /// <response code="200">Roles retrieved successfully.</response>
        /// <response code="401">Unauthorized if the caller lacks admin privileges.</response>
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllRolesWithPermissions()
        {
            var roles = await roleService.GetAllRolesWithPermissionsAsync();
            return Ok(roles);
        }

        /// <summary>
        /// Removes a permission from a role.
        /// </summary>
        /// <param name="roleId">The ID of the role to remove the permission from.</param>
        /// <param name="permissionId">The ID of the permission to remove.</param>
        /// <returns>No content on successful removal.</returns>
        /// <response code="204">Permission removed successfully.</response>
        /// <response code="401">Unauthorized if the caller lacks admin privileges.</response>
        /// <response code="500">Server error if the removal fails.</response>
        /// <remarks>If the permission is not assigned, the request is ignored.</remarks>
        [HttpDelete("{roleId}/permissions/{permissionId}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemovePermission(int roleId, int permissionId)
        {
            await roleService.RemovePermissionFromRoleAsync(roleId, permissionId);
            return NoContent();
        }

        /// <summary>
        /// Retrieves the permissions assigned to a specific role.
        /// </summary>
        /// <param name="roleId">The ID of the role to retrieve permissions for.</param>
        /// <returns>A list of permission names assigned to the role.</returns>
        /// <response code="200">Permissions retrieved successfully.</response>
        /// <response code="401">Unauthorized if the caller lacks admin privileges.</response>
        [HttpGet("{roleId}/permissions")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetPermissions(int roleId)
        {
            var permissions = await roleService.GetPermissionsForRoleAsync(roleId);
            return Ok(permissions);
        }
    }
}