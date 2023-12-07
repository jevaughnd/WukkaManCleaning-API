using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WukkaManCleaning_API.Data;
using WukkaManCleaning_API.Models;

namespace WukkaManCleaning_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    
    public class EmployeeAPIController : ControllerBase
    {
        private readonly IdentityApplicationDbContext _cxt;
        public EmployeeAPIController(IdentityApplicationDbContext cxt)
        {
            this._cxt = cxt;
        }


		///EMPLOYEE ENDPOINTS
		[HttpGet("Employee")]
        public IActionResult GetEmplyees()
        {
            var employees = _cxt.Employees.ToList();

            if (employees == null)
            {
                return BadRequest();
            }
            return Ok(employees);
        }

        
        
        //Finds Individual Record By Id
        [HttpGet("{id}")]
        public IActionResult GetEmployeeById(int id)
        {
            var employee = _cxt.Employees.FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        

        //Create Record
        [HttpPost("EmployeePost")]
        public IActionResult CreateEmployee([FromBody] Employee values)
        {
            _cxt.Employees.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetEmployeeById), new { id = values.Id }, values);
        }



        //Edit Record
        [HttpPut("EmployeePut")]
        public IActionResult UpdateCustomer([FromBody] Employee values)
        {
            if (values.Id == null)
            {
                return NotFound();
            }
            _cxt.Employees.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetEmployeeById), new { id = values.Id }, values);
        }




        //Delete Individual Record By Id
        [HttpDelete("{id}")]
        public IActionResult DeleteEmployeeById(int id)
        {
            var employee = _cxt.Employees.FirstOrDefault(x => x.Id == id);

            if (employee == null)
            {
                return NotFound();
            }
            _cxt.Employees.Remove(employee);
            _cxt.SaveChanges();
            return Ok(employee);
        }



        //-------------------------------
        //Employee Screen
        //gets worker/Employee  Information by using Id, and work Id
        [HttpGet("Employee/{workId}/{id}")]
        public IActionResult GetEmployeeByWorkIdAndId(int workId, int id)
        {
            var employee = _cxt.Employees.FirstOrDefault(e => e.WorkId == workId && e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }










    }
}
