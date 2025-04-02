using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystemTask.Infrastructure.Models
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string EmployeeCode { get; set; } 
        public string Name { get; set; }
        public string JobTitle { get; set; }
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public int? UserId { get; set; }    
        public User? User { get; set; }
        public SalaryDetails ?SalaryDetails { get; set; } 
    }
}
