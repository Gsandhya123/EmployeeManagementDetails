using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.BusinessLogic.Interfaces;
using System;
using EmployeeManagement.Models;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeManagementController : Controller
    {
        private readonly IEmployeeManageService _employeeMangeServices;

        public EmployeeManagementController(IEmployeeManageService employeeManageServices)
        {
            _employeeMangeServices = employeeManageServices;
        }
        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            var employees = _employeeMangeServices.GetAllEmployees();
            return Ok(employees);
        }

        [HttpPost]
        public IActionResult AddEmployee([FromBody] EmployeeManagementCreateDto employeeCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate the role and required fields based on the role
            if (string.IsNullOrEmpty(employeeCreateDto.Role))
            {
                return BadRequest("Role must be specified.");
            }

            if (employeeCreateDto.Role == "Employee" && employeeCreateDto.PayPerHour <= 0)
            {
                return BadRequest("PayPerHour must be greater than 0 for an Employee.");
            }

            if (employeeCreateDto.Role == "Supervisor" && employeeCreateDto.AnnualSalary <= 0)
            {
                return BadRequest("AnnualSalary must be greater than 0 for a Supervisor.");
            }

            if (employeeCreateDto.Role == "Manager" && (employeeCreateDto.AnnualSalary <= 0 || employeeCreateDto.MaxExpenseAmount < 0))
            {
                return BadRequest("AnnualSalary must be greater than 0 and MaxExpenseAmount cannot be negative for a Manager.");
            }

            try
            {
                // Map EmployeeCreateDto to EmployeeDto
                var employeeDto = new EmployeeManageDto
                {
                    FirstName = employeeCreateDto.FirstName,
                    LastName = employeeCreateDto.LastName,
                    Address = employeeCreateDto.Address,
                    PayPerHour = employeeCreateDto.PayPerHour,
                    AnnualSalary = employeeCreateDto.AnnualSalary,
                    MaxExpenseAmount = employeeCreateDto.MaxExpenseAmount,
                    Role = employeeCreateDto.Role
                };

                _employeeMangeServices.AddEmployee(employeeDto);
                return Ok("Employee added successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
