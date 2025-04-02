using LoginSystemTask.Application.DTO;
using LoginSystemTask.Application.Interfaces.IServices.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoginSystemTask.API.Controllers
{
    /// <summary>
    /// Controller for handling authentication and user role management.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        /// <summary>
        /// Authenticates a user and generates a JWT token.
        /// </summary>
        /// <param name="dto">The login credentials.</param>
        /// <returns>A JWT token if authentication is successful.</returns>
        /// <response code="200">Returns the generated JWT token.</response>
        /// <response code="401">Invalid username or password.</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            try
            {
                var token = await authService.LoginAsync(dto);
                return Ok(new { Token = token });
            }
            catch (Exception)
            {
                return Unauthorized("Invalid username or password");
            }
        }
        /// <summary>
        /// Assigns a role to a specified user.
        /// </summary>
        /// <param name="userId">The ID of the user to whom the role will be assigned.</param>
        /// <param name="roleName">The role to be assigned to the user.</param>
        /// <returns>A success message if the role is assigned.</returns>
        /// <response code="200">Role assigned successfully.</response>
        /// <response code="400">Invalid input or user/role not found.</response>
        /// <response code="401">Unauthorized if the caller lacks admin privileges.</response>
        [HttpPost("users/{userId}/assign-role")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AssignRole(int userId, string roleName)
        {
            try
            {
                await authService.AssignRoleToUserAsync(userId, roleName);
                return Ok("Role assigned successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Updates the role of a specified user.
        /// </summary>
        /// <param name="userId">The ID of the user whose role is to be updated.</param>
        /// <param name="roleName">The new role to be assigned to the user.</param>
        /// <returns>A success message if the role is updated.</returns>
        /// <response code="200">User role updated successfully.</response>
        /// <response code="400">Invalid input or user not found.</response>
        /// <response code="401">Unauthorized if the caller lacks admin privileges.</response>
        [HttpPut("users/{userId}/role")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateUserRole(int userId, string roleName)
        {
            try
            {
                await authService.UpdateUserRoleAsync(userId, roleName);
                return Ok("User role updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
