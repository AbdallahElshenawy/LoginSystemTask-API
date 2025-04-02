using LoginSystemTask.Application.DTO.Employee;
using LoginSystemTask.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginSystemTask.API.Controllers
{
    /// <summary>
    /// Controller for managing employee operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController(IEmployeeService employeeService) : ControllerBase
    {
        /// <summary>
        /// Creates a new employee in the system.
        /// </summary>
        /// <param name="dto">The employee details to create.</param>
        /// <returns>The created employee details.</returns>
        /// <response code="201">Employee created successfully.</response>
        /// <response code="400">Invalid input (e.g., missing fields, duplicate EmployeeCode, invalid UserId).</response>
        /// <response code="401">Unauthorized if the caller lacks admin privileges.</response>
        /// <response code="500">Server error if the employee creation fails.</response>
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeAddDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var employee = await employeeService.AddEmployeeAsync(dto);
                return CreatedAtAction(nameof(GetEmployeeByCode), new { employeeCode = employee.EmployeeCode }, employee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing employee.
        /// </summary>
        /// <param name="id">The GUID of the employee to update.</param>
        /// <param name="dto">The updated employee details.</param>
        /// <returns>The updated employee details.</returns>
        /// <response code="200">Employee updated successfully.</response>
        /// <response code="400">Invalid input (e.g., ID mismatch, invalid UserId, attempt to change EmployeeCode).</response>
        /// <response code="401">Unauthorized if the caller lacks admin privileges.</response>
        /// <response code="404">Employee not found.</response>
        /// <response code="500">Server error if the update fails.</response>
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] EmployeeUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var employee = await employeeService.UpdateEmployeeAsync(id, dto);
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes an employee by ID.
        /// </summary>
        /// <param name="id">The GUID of the employee to delete.</param>
        /// <returns>No content on successful deletion.</returns>
        /// <response code="204">Employee deleted successfully.</response>
        /// <response code="400">Invalid request (e.g., employee is active).</response>
        /// <response code="401">Unauthorized if the caller lacks admin privileges.</response>
        /// <response code="404">Employee not found.</response>
        /// <response code="500">Server error if the deletion fails.</response>
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            try
            {
                await employeeService.DeleteEmployeeAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves an employee by their employee code.
        /// </summary>
        /// <param name="employeeCode">The unique code of the employee to retrieve.</param>
        /// <returns>The employee details if found.</returns>
        /// <response code="200">Employee retrieved successfully.</response>
        /// <response code="404">Employee not found.</response>
        /// <response code="400">Invalid request (e.g., server error).</response>
        /// <remarks>Currently no authentication required; consider adding authorization if needed.</remarks>
        [HttpGet("{employeeCode}")]
        // [Authorize] // Uncomment if authentication is required
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEmployeeByCode(string employeeCode)
        {
            try
            {
                var employee = await employeeService.GetEmployeeByCodeAsync(employeeCode);
                if (employee == null)
                    return NotFound("Employee not found");
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a filtered list of employees.
        /// </summary>
        /// <param name="name">Filter by employee name (optional).</param>
        /// <param name="jobTitle">Filter by job title (optional).</param>
        /// <param name="minSalary">Filter by minimum salary (optional).</param>
        /// <param name="maxSalary">Filter by maximum salary (optional).</param>
        /// <param name="expandSalaryDetails">Include salary details in the response if true (default: false).</param>
        /// <returns>A list of employees matching the filters.</returns>
        /// <response code="200">Employees retrieved successfully.</response>
        /// <response code="400">Invalid request (e.g., server error).</response>
        /// <remarks>Currently no authentication required; consider adding authorization if needed.</remarks>
        [HttpGet]
        // [Authorize] // Uncomment if authentication is required
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetFilteredEmployees(
            [FromQuery] string? name = null,
            [FromQuery] string? jobTitle = null,
            [FromQuery] decimal? minSalary = null,
            [FromQuery] decimal? maxSalary = null,
            [FromQuery] bool expandSalaryDetails = false)
        {
            try
            {
                var employees = await employeeService.GetFilteredEmployeesAsync(name, jobTitle, minSalary, maxSalary, expandSalaryDetails);
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}