using System.ComponentModel.DataAnnotations;

namespace LoginSystemTask.Application.DTO.Employee
{
    public class EmployeeAddDto
    {
        [Required(ErrorMessage = "EmployeeCode is required.")]
        [StringLength(50, ErrorMessage = "EmployeeCode cannot exceed 50 characters.")]
        public string EmployeeCode { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "JobTitle is required.")]
        [StringLength(100, ErrorMessage = "JobTitle cannot exceed 100 characters.")]
        public string JobTitle { get; set; }

        [Required(ErrorMessage = "Salary is required.")]
        [Range(0.01, 9999999.99, ErrorMessage = "Salary must be between 0.01 and 9,999,999.99.")]
        public decimal Salary { get; set; }

        public bool IsActive { get; set; } = true;

        public int? UserId { get; set; }

        public SalaryDetailsDto? SalaryDetails { get; set; } 
    }
}