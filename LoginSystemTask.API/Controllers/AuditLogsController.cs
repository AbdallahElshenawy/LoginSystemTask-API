using LoginSystemTask.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginSystemTask.API.Controllers
{
    /// <summary>
    /// Controller for retrieving audit logs.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuditLogsController(IAuditLogService auditLogService) : ControllerBase
    {
        /// <summary>
        /// Retrieves audit logs with optional filters.
        /// </summary>
        /// <param name="action">Filter by action type (e.g., "Create", "Update", "Delete"). Optional.</param>
        /// <param name="entityName">Filter by entity name (e.g., "User", "Employee", "Role"). Optional.</param>
        /// <param name="userId">Filter by the ID of the user who performed the action. Optional.</param>
        /// <param name="startDate">Filter logs on or after this date. Optional.</param>
        /// <param name="endDate">Filter logs on or before this date. Optional.</param>
        /// <returns>A list of audit logs matching the specified filters.</returns>
        /// <response code="200">Audit logs retrieved successfully.</response>
        /// <response code="401">Unauthorized if the caller lacks admin privileges.</response>
        /// <response code="500">Server error if the retrieval fails.</response>
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAuditLogs(
            [FromQuery] string? action = null,
            [FromQuery] string? entityName = null,
            [FromQuery] int? userId = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var logs = await auditLogService.GetAuditLogsAsync(action, entityName, userId, startDate, endDate);
            return Ok(logs);
        }

    }
}