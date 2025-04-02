using LoginSystemTask.Application.Interfaces.IServices.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace LoginSystemTask.API.Controllers
{
    /// <summary>
    /// Controller for managing permissions.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController(IPermissionService permissionService) : ControllerBase
    {
        /// <summary>
        /// Creates a new permission.
        /// </summary>
        /// <param name="name">The name of the permission to create.</param>
        /// <returns>The created permission.</returns>
        /// <response code="201">Permission created successfully.</response>
        /// <response code="400">Invalid input (e.g., empty name, duplicate permission).</response>
        /// <response code="401">Unauthorized if the caller lacks admin privileges.</response>
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreatePermission([FromBody] string name)
        {
            try
            {
                var permission = await permissionService.CreatePermissionAsync(name);
                return CreatedAtAction(nameof(GetPermissionById), new { id = permission.Id }, permission);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a permission by its ID.
        /// </summary>
        /// <param name="id">The ID of the permission.</param>
        /// <returns>The requested permission.</returns>
        /// <response code="200">Permission retrieved successfully.</response>
        /// <response code="404">Permission not found.</response>
        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPermissionById(int id)
        {
            try
            {
                var permission = await permissionService.GetPermissionByIdAsync(id);
                return Ok(permission);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all permissions.
        /// </summary>
        /// <returns>A list of all permissions.</returns>
        /// <response code="200">Permissions retrieved successfully.</response>
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPermissions()
        {
            var permissions = await permissionService.GetAllPermissionsAsync();
            return Ok(permissions);
        }

        /// <summary>
        /// Updates an existing permission.
        /// </summary>
        /// <param name="id">The ID of the permission to update.</param>
        /// <param name="name">The new name of the permission.</param>
        /// <response code="204">Permission updated successfully.</response>
        /// <response code="400">Invalid input (e.g., empty name, duplicate permission).</response>
        /// <response code="401">Unauthorized if the caller lacks admin privileges.</response>
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdatePermission(int id, [FromBody] string name)
        {
            try
            {
                await permissionService.UpdatePermissionAsync(id, name);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a permission by ID.
        /// </summary>
        /// <param name="id">The ID of the permission to delete.</param>
        /// <response code="204">Permission deleted successfully.</response>
        /// <response code="404">Permission not found.</response>
        /// <response code="401">Unauthorized if the caller lacks admin privileges.</response>
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeletePermission(int id)
        {
            try
            {
                await permissionService.DeletePermissionAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
