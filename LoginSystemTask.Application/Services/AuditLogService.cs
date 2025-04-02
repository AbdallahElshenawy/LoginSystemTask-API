using LoginSystemTask.Application.Interfaces.IServices;
using LoginSystemTask.Infrastructure.Interfaces;
using LoginSystemTask.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.IO;
using iText.Kernel.Exceptions;
namespace LoginSystemTask.Infrastructure.Repositories
{
    public class AuditLogService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) : IAuditLogService
    {
        public async Task LogAsync(string action, string entityName, string entityId, string? details = null)
        {
            var log = new AuditLog
            {
                Action = action,
                EntityName = entityName,
                EntityId = entityId,
                Timestamp = DateTime.UtcNow,
                UserId = GetCurrentUserId(),
                Details = details
            };
            await unitOfWork.AuditLogs.AddAsync(log);
        }
        public async Task<IEnumerable<AuditLog>> GetAuditLogsAsync(
        string? action = null, string? entityName = null, int? userId = null,
        DateTime? startDate = null, DateTime? endDate = null)
        {
            return await unitOfWork.AuditLogs.GetAllAsync(
                criteria: log =>
                    (action == null || log.Action == action) &&
                    (entityName == null || log.EntityName == entityName) &&
                    (userId == null || log.UserId == userId) &&
                    (startDate == null || log.Timestamp >= startDate) &&
                    (endDate == null || log.Timestamp <= endDate),
                orderBy: log => log.Timestamp,
                orderByDirection: OrderByDirection.Descending);
        }
        private int GetCurrentUserId()
        {
            var userIdClaim = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                throw new InvalidOperationException("Unable to retrieve authenticated user ID.");
            return userId;
        }
    }
}