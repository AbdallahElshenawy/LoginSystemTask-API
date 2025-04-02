using LoginSystemTask.Application.DTO.User;
using LoginSystemTask.Application.Interfaces.IServices;
using LoginSystemTask.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginSystemTask.API.Controllers
{
    /// <summary>
    /// Controller for managing user operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService) : ControllerBase
    {
        /// <summary>
        /// Creates a new user in the system.
        /// </summary>
        /// <param name="dto">The user details to create.</param>
        /// <returns>The created user details.</returns>
        /// <response code="201">User created successfully.</response>
        /// <response code="400">Invalid input (e.g., missing username or password, invalid role ID).</response>
        /// <response code="401">Unauthorized if the caller lacks admin privileges.</response>
        /// <response code="409">Conflict if the username already exists.</response>
        /// <response code="500">Server error if the user creation fails.</response>
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddUser([FromBody] UserAddDto dto)
        {
            var user = await userService.AddUserAsync(dto);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="dto">The updated user details.</param>
        /// <returns>The updated user details.</returns>
        /// <response code="200">User updated successfully.</response>
        /// <response code="400">Invalid input (e.g., ID mismatch, missing fields, invalid role ID).</response>
        /// <response code="401">Unauthorized if the caller lacks admin privileges.</response>
        /// <response code="404">User not found.</response>
        /// <response code="409">Conflict if the updated username already exists.</response>
        /// <response code="500">Server error if the update fails.</response>
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto dto)
        {
            var user = await userService.UpdateUserAsync(id, dto);
            return Ok(user);
        }

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>No content on successful deletion.</returns>
        /// <response code="204">User deleted successfully.</response>
        /// <response code="401">Unauthorized if the caller lacks admin privileges.</response>
        /// <response code="404">User not found.</response>
        /// <response code="409">Conflict if the user is linked to an employee.</response>
        /// <response code="500">Server error if the deletion fails.</response>
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await userService.DeleteUserAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Retrieves a user by ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>The user details if found.</returns>
        /// <response code="200">User retrieved successfully.</response>
        /// <response code="401">Unauthorized if the caller is not authenticated.</response>
        /// <response code="404">User not found.</response>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await userService.GetUserByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        /// <summary>
        /// Retrieves all users in the system.
        /// </summary>
        /// <returns>A list of all users.</returns>
        /// <response code="200">Users retrieved successfully.</response>
        /// <remarks>Currently no authentication required; consider adding authorization if needed.</remarks>
        [HttpGet]
        // [Authorize] // Uncomment if you want to require authentication
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await userService.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Retrieves a user by username.
        /// </summary>
        /// <param name="username">The username of the user to retrieve.</param>
        /// <returns>The user details if found.</returns>
        /// <response code="200">User retrieved successfully.</response>
        /// <response code="401">Unauthorized if the caller is not authenticated.</response>
        /// <response code="404">User not found.</response>
        [HttpGet("username/{username}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var user = await userService.GetUserByUsernameAsync(username);
            return user == null ? NotFound() : Ok(user);
        }
    }
}