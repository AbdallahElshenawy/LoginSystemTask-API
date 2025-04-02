using LoginSystemTask.Application.DTO.Employee;
using LoginSystemTask.Application.Interfaces.IRepositories;
using LoginSystemTask.Application.Interfaces.IServices;
using LoginSystemTask.Infrastructure.Interfaces;
using LoginSystemTask.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginSystemTask.Application.Services
{
    public class EmployeeService(IUnitOfWork unitOfWork, IEmployeeRepository employeeRepository, IAuditLogService auditLogService) : IEmployeeService
    {
        public async Task<EmployeeResponseDto> AddEmployeeAsync(EmployeeAddDto dto)
        {
            await ValidateEmployeeCodeAsync(dto.EmployeeCode, isNew: true);
            if (dto.UserId.HasValue)
            {
                await ValidateUserIdAsync(dto.UserId.Value); 
            }

            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                EmployeeCode = dto.EmployeeCode,
                Name = dto.Name,
                JobTitle = dto.JobTitle,
                Salary = dto.Salary,
                IsActive = dto.IsActive,
                UserId = dto.UserId 
            };

            if (dto.SalaryDetails != null)
            {
                employee.SalaryDetails = new SalaryDetails
                {
                    EmployeeId = employee.Id,
                    Bonus = dto.SalaryDetails.Bonus,
                    Deductions = dto.SalaryDetails.Deductions,
                    Tax = dto.SalaryDetails.Tax
                };
            }

            try
            {
                await unitOfWork.Employees.AddAsync(employee);
                await auditLogService.LogAsync("Create", "Employee", employee.Id.ToString(), $"EmployeeCode: {dto.EmployeeCode}");
                await unitOfWork.Complete();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Failed to save employee.", ex);
            }

            return MapToResponseDto(employee, expandSalaryDetails: true);
        }

        public async Task<EmployeeResponseDto> UpdateEmployeeAsync(Guid id, EmployeeUpdateDto dto)
        {
            if (id != dto.Id)
                throw new Exception("ID in URL must match ID in body");

            var employee = await GetEmployeeByIdAsync(id);
            await ValidateEmployeeCodeAsync(dto.EmployeeCode, isNew: false, existingEmployee: employee);
            if (dto.UserId.HasValue)
            {
                await ValidateUserIdAsync(dto.UserId.Value); 
            }

            employee.Name = dto.Name;
            employee.JobTitle = dto.JobTitle;
            employee.Salary = dto.Salary;
            employee.IsActive = dto.IsActive;
            employee.UserId = dto.UserId;

            if (dto.SalaryDetails != null)
            {
                if (employee.SalaryDetails == null)
                {
                    employee.SalaryDetails = new SalaryDetails { EmployeeId = id };
                }
                employee.SalaryDetails.Bonus = dto.SalaryDetails.Bonus;
                employee.SalaryDetails.Deductions = dto.SalaryDetails.Deductions;
                employee.SalaryDetails.Tax = dto.SalaryDetails.Tax;
            }
            else if (employee.SalaryDetails != null)
            {
                employee.SalaryDetails = null;
            }

            try
            {
                await unitOfWork.Employees.UpdateAsync(employee);
                await auditLogService.LogAsync("Update", "Employee", id.ToString(),
                            $"Name: {dto.Name}, JobTitle: {dto.JobTitle}, Salary: {dto.Salary}");
                await unitOfWork.Complete();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Failed to save employee changes.", ex);
            }

            return MapToResponseDto(employee, expandSalaryDetails: true);
        }

        public async Task DeleteEmployeeAsync(Guid id)
        {
            var employee = await GetEmployeeByIdAsync(id);
            if (employee.IsActive)
                throw new Exception("Cannot delete active employee");

            try
            {
                await unitOfWork.Employees.DeleteAsync(id);
                await auditLogService.LogAsync("Delete", "Employee", id.ToString(),
                            $"EmployeeCode: {employee.EmployeeCode}");
                await unitOfWork.Complete();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Failed to delete employee.", ex);
            }
        }

        public async Task<EmployeeResponseDto?> GetEmployeeByCodeAsync(string employeeCode)
        {
            var employee = await employeeRepository.GetEmployeeByCodeAsync(employeeCode);
            return employee == null ? null : MapToResponseDto(employee, expandSalaryDetails: true);
        }

        public async Task<IEnumerable<EmployeeResponseDto>> GetFilteredEmployeesAsync(
            string? name = null,
            string? jobTitle = null,
            decimal? minSalary = null,
            decimal? maxSalary = null,
            bool expandSalaryDetails = false)
        {
            var employees = await employeeRepository.GetFilteredEmployeesAsync(name, jobTitle, minSalary, maxSalary, expandSalaryDetails);
            return employees.Select(e => MapToResponseDto(e, expandSalaryDetails));
        }

        private async Task<Employee> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await unitOfWork.Employees.GetByIdAsync(id);
            if (employee == null)
                throw new Exception("Employee not found");
            return employee;
        }

        private async Task ValidateEmployeeCodeAsync(string employeeCode, bool isNew, Employee? existingEmployee = null)
        {
            var existing = await employeeRepository.GetEmployeeByCodeAsync(employeeCode);
            if (isNew && existing != null)
                throw new Exception("EmployeeCode must be unique");
            if (!isNew && existing != null && existing.Id != existingEmployee?.Id)
                throw new Exception("EmployeeCode must be unique and cannot be changed to an existing code");
            if (!isNew && existingEmployee?.EmployeeCode != employeeCode)
                throw new Exception("EmployeeCode cannot be changed");
        }

        private async Task ValidateUserIdAsync(int userId) 
        {
            var userExists = await unitOfWork.Users.GetByIdAsync(userId); 
            if (userExists == null)
                throw new Exception($"User with ID {userId} does not exist.");
        }

        private EmployeeResponseDto MapToResponseDto(Employee employee, bool expandSalaryDetails)
        {
            return new EmployeeResponseDto
            {
                Id = employee.Id,
                EmployeeCode = employee.EmployeeCode,
                Name = employee.Name,
                JobTitle = employee.JobTitle,
                IsActive = employee.IsActive,
                UserId = employee.UserId, 
                SalaryDetails = expandSalaryDetails
                    ? new SalaryDetailsDto
                    {
                        Salary = employee.Salary,
                        Bonus = employee.SalaryDetails?.Bonus ?? 0,
                        Deductions = employee.SalaryDetails?.Deductions ?? 0,
                        Tax = employee.SalaryDetails?.Tax ?? 0,
                        NetSalary = employee.SalaryDetails?.NetSalary ?? employee.Salary
                    }
                    : null
            };
        }
    }
}