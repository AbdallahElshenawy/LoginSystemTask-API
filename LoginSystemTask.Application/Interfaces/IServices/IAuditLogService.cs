using LoginSystemTask.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystemTask.Application.Interfaces.IServices
{
    public interface IAuditLogService
    {
        Task LogAsync(string action, string entityName, string entityId, string? details = null);
        Task<IEnumerable<AuditLog>> GetAuditLogsAsync(
       string? action = null, string? entityName = null, int? userId = null,
       DateTime? startDate = null, DateTime? endDate = null);
    }
    
}
