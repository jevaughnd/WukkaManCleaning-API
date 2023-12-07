using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WukkaManCleaning_API.Data;
using WukkaManCleaning_API.Models;

namespace WukkaManCleaning_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpClockShiftAPIController : ControllerBase
    {

        private readonly IdentityApplicationDbContext _cxt;


        public EmpClockShiftAPIController(IdentityApplicationDbContext cxt)
        {
            this._cxt = cxt;
        }


        [HttpGet("ClockShift")]
        public IActionResult GetEmplyeeClockShift()
        {
            var shifts = _cxt.EmplyeeClockShifts.Include(e => e.Employee).ToList();

            if (shifts == null)
            {
                return BadRequest();
            }
            return Ok(shifts);
        }


        //Finds Individual Record By Id
        [HttpGet("{id}")]
        public IActionResult GetClockShiftById(int id)
        {
            var shift = _cxt.EmplyeeClockShifts.Include(s => s.Employee).FirstOrDefault(x => x.Id == id);

            if (shift == null)
            {
                return NotFound();
            }
            return Ok(shift);
        }

        //Create Record
        [HttpPost("ClockShiftPost")]
        public IActionResult CreateClockShift([FromBody] EmplyeeClockShift values)
        {
            _cxt.EmplyeeClockShifts.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetClockShiftById), new { id = values.Id }, values);
        }

        //Edit Record
        [HttpPut("ClockShiftPut")]
        public IActionResult UpdateClockShift([FromBody] EmplyeeClockShift values)
        {
            if (values.Id == null)
            {
                return NotFound();
            }
            _cxt.EmplyeeClockShifts.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetClockShiftById), new { id = values.Id }, values);
        }




        //Delete Individual Record By Id
        [HttpDelete("{id}")]
        public IActionResult DeleteClockShiftById(int id)
        {
            var shift = _cxt.EmplyeeClockShifts.Include(s => s.Employee).FirstOrDefault(x => x.Id == id);

            if (shift == null)
            {
                return NotFound();
            }
            _cxt.EmplyeeClockShifts.Remove(shift);
            _cxt.SaveChanges();
            return Ok(shift);
        }






        //========================================================================================================================

        //EMPLOYEE ENDPOINTS
        [HttpGet("Employee")]
        public IActionResult GetEmplyees()
        {
            var employee = _cxt.Employees.ToList();

            if (employee == null)
            {
                return BadRequest();
            }
            return Ok(employee);

        }


        //Finds Individual Record By Id
        [HttpGet("Employee/{Id}")]
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
        public IActionResult UpdateCustomer(int id,[FromBody] Employee values)
        {
            var employee = _cxt.Employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            _cxt.Employees.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetEmployeeById), new { id = values.Id }, values);
        }
    }
}
