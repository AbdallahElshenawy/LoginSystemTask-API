namespace LoginSystemTask.Application.DTO.Employee
{
    public class EmployeeResponseDto
    {
        public Guid Id { get; set; }
        public string EmployeeCode { get; set; }
        public string Name { get; set; }
        public string JobTitle { get; set; }
        public bool IsActive { get; set; }
        public int? UserId { get; set; }
        public SalaryDetailsDto? SalaryDetails { get; set; }
    }

    public class SalaryDetailsDto
    {
        public decimal Salary { get; set; }
        public decimal Bonus { get; set; }
        public decimal Deductions { get; set; }
        public decimal Tax { get; set; }
        public decimal NetSalary { get; set; }
    }
}