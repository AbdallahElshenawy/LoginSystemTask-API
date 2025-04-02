using LoginSystemTask.Application.DTO.Employee;
using LoginSystemTask.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystemTask.Application.Interfaces.IServices
{
    public interface IEmployeeService
    {
        Task<EmployeeResponseDto> AddEmployeeAsync(EmployeeAddDto dto);
        Task<EmployeeResponseDto> UpdateEmployeeAsync(Guid id, EmployeeUpdateDto dto);
        Task DeleteEmployeeAsync(Guid id);
        Task<EmployeeResponseDto?> GetEmployeeByCodeAsync(string employeeCode);
        Task<IEnumerable<EmployeeResponseDto>> GetFilteredEmployeesAsync(
            string? name = null,
            string? jobTitle = null,
            decimal? minSalary = null,
            decimal? maxSalary = null,
            bool expandSalaryDetails = false);
    }
}
