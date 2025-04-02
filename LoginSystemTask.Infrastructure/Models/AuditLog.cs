using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystemTask.Infrastructure.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string EntityName { get; set; }
        public string EntityId { get; set; } 
        public DateTime Timestamp { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        public string? Details { get; set; }
    }
}
