using Cafe_NET_API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cafe_NET_API.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        public EmployeeController() 
        { 
        
        }

        [HttpGet]
        public async Task<IActionResult> Cafes([FromQuery] string location)
        {
            try
            {
                return Ok();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex);
            }

        }

        //[HttpGet]
        //public async Task<IActionResult> Employees([FromQuery] string cafe)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(cafe))
        //        {

        //            _appDbContext.

        //            return Ok(_appDbContext.);
        //        }
        //        else 
        //        { 
                
                
        //        }

        //    }
        //    catch (Exception ex) 
        //    { 
            
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> AddEmployee(Employee employee)
        {
            try
            {
             

                return Ok(employee);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex);
            }
        }
    }
}
