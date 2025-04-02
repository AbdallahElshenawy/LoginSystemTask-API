using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystemTask.Infrastructure.Models
{
    public class SalaryDetails
    {
        public int Id { get; set; }
        public Guid EmployeeId { get; set; }
        public decimal Bonus { get; set; }
        public decimal Deductions { get; set; }
        public decimal Tax { get; set; }
        public decimal NetSalary => (Employee?.Salary ?? 0) + Bonus - (Deductions + Tax);
        public Employee Employee { get; set; }
    }
}
