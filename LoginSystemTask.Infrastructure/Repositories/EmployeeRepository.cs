using LoginSystemTask.Application.Interfaces.IRepositories;
using LoginSystemTask.Infrastructure.Data;
using LoginSystemTask.Infrastructure.Interfaces;
using LoginSystemTask.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystemTask.Infrastructure.Repositories
{
    public class EmployeeRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : BaseRepository<Employee>(context), IEmployeeRepository
    {
        public async Task<Employee?> GetEmployeeByCodeAsync(string employeeCode)
        {
            var employees = await unitOfWork.Employees.GetAllAsync(e => e.EmployeeCode == employeeCode);
            return employees.FirstOrDefault();
        }

        public async Task<IEnumerable<Employee>> GetFilteredEmployeesAsync(
            string? name = null,
            string? jobTitle = null,
            decimal? minSalary = null,
            decimal? maxSalary = null,
            bool expandSalaryDetails = false)
        {
            Expression<Func<Employee, bool>>? criteria = null;

            if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(jobTitle) || minSalary.HasValue || maxSalary.HasValue)
            {
                criteria = e =>
                    (string.IsNullOrEmpty(name) || e.Name.Contains(name)) &&
                    (string.IsNullOrEmpty(jobTitle) || e.JobTitle.Contains(jobTitle)) &&
                    (!minSalary.HasValue || e.Salary >= minSalary.Value) &&
                    (!maxSalary.HasValue || e.Salary <= maxSalary.Value);
            }

            // Use includes if salary details are expanded (e.g., related data)
            string[]? includes = expandSalaryDetails ? new[] { "User" } : null; // Example: expand related User data if needed

            return await GetAllAsync(criteria: criteria, includes: includes);
        }
    }
}