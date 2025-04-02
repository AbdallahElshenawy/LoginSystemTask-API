using LoginSystemTask.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystemTask.Infrastructure.Interfaces.IRepositories
{
    public interface IAuditLogRepository : IBaseRepository<AuditLog>
    {
        Task<IEnumerable<AuditLog>> GetByEntityIdAsync(Guid entityId);
        Task<IEnumerable<AuditLog>> GetByUserIdAsync(int userId);
        Task<IEnumerable<AuditLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
