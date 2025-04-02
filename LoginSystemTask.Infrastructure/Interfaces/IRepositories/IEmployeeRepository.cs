using LoginSystemTask.Infrastructure.Interfaces;
using LoginSystemTask.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystemTask.Application.Interfaces.IRepositories
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        Task<Employee?> GetEmployeeByCodeAsync(string employeeCode);
        Task<IEnumerable<Employee>> GetFilteredEmployeesAsync(
            string? name = null,
            string? jobTitle = null,
            decimal? minSalary = null,
            decimal? maxSalary = null,
            bool expandSalaryDetails = false);
    }
}
