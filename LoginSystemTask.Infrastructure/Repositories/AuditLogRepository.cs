using LoginSystemTask.Infrastructure.Data;
using LoginSystemTask.Infrastructure.Interfaces.IRepositories;
using LoginSystemTask.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystemTask.Infrastructure.Repositories
{
    public class AuditLogRepository(ApplicationDbContext context) : BaseRepository<AuditLog>(context), IAuditLogRepository
    {
        public async Task<IEnumerable<AuditLog>> GetByEntityIdAsync(Guid entityId)
        {
            return await context.AuditLogs
                .Where(al => al.EntityId == entityId.ToString())
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetByUserIdAsync(int userId)
        {
            return await context.AuditLogs
                .Where(al => al.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await context.AuditLogs
                .Where(al => al.Timestamp >= startDate && al.Timestamp <= endDate)
                .ToListAsync();
        }
    }
}
