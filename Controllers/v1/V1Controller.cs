﻿using Cafe_NET_API.Entities;
using Cafe_NET_API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cafe_NET_API.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class V1Controller : ControllerBase
    {
        private readonly ICafeEmployeeService _cafeEmployeeService;

        public V1Controller(ICafeEmployeeService cafeEmployeeService) 
        { 
            _cafeEmployeeService = cafeEmployeeService;
        }

        [HttpGet("Employees")]
        public async Task<IActionResult> Employees([FromQuery]string? cafe)
        {
            try
            {
                return Ok(await _cafeEmployeeService.GetEmployees(cafe));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex);
            }

        }

        [HttpPost("Employee"), HttpPut("Employee"), HttpDelete("Employee")]
        public async Task<IActionResult> Employee(EmployeeCreateUpdate employee)
        {
            try
            {
                if (HttpContext.Request.Method == "POST" && string.IsNullOrEmpty(employee.Id))
                {
                    return Ok(await _cafeEmployeeService.CreateEmployee(employee));
                }
                else if (HttpContext.Request.Method == "PUT" && !string.IsNullOrEmpty(employee.Id))
                {
                    return Ok(await _cafeEmployeeService.UpdateEmployee(employee));
                }
                else if (HttpContext.Request.Method == "DELETE" && !string.IsNullOrEmpty(employee.Id))
                {
                    return Ok(await _cafeEmployeeService.DeleteEmployee(employee.Id));
                }
                else
                {
                    throw new Exception("Please check Method & Employee Id");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex);
            }

        }

    }
}
