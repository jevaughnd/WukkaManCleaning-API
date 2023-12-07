using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WukkaManCleaning_API.Data;
using WukkaManCleaning_API.Models;

namespace WukkaManCleaning_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeTaskAPIController : ControllerBase
    {

        private readonly IdentityApplicationDbContext _cxt;
        public EmployeeTaskAPIController(IdentityApplicationDbContext cxt)
        {
            this._cxt = cxt;
        }

        //EMPLOYEETASK END POINTS

        [HttpGet("EmployeeTask")]
        public IActionResult GetEmpTasks()
        {
            var emps = _cxt.EmployeesTasks.Include(e => e.Employee)
                                           .Include(e => e.Task).ToList();

            if (emps == null)
            {
                return BadRequest();
            }
            return Ok(emps);
        }


        //Finds Individual Record By Id
        [HttpGet("{id}")]
        public IActionResult GetEmpTaskById(int id)
        {
            var emp = _cxt.EmployeesTasks.Include(e => e.Employee)
                                          .Include(e => e.Task).FirstOrDefault(x => x.Id == id);

            if (emp == null)
            {
                return NotFound();
            }
            return Ok(emp);
        }


        //Create Record
        [HttpPost("EmployeeTaskPost")]
        public IActionResult CreateEmployeeTask([FromBody] EmployeeTask values)
        {
            _cxt.EmployeesTasks.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetEmpTaskById), new { id = values.Id }, values);
        }


        //Edit Record
        [HttpPut("EmployeeTaskPut")]
        public IActionResult UpdateCustomer([FromBody] EmployeeTask values)
        {
            if (values.Id == null)
            {
                return NotFound();
            }
            _cxt.EmployeesTasks.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetEmpTaskById), new { id = values.Id }, values);
        }


        //Delete Individual Record By Id
        [HttpDelete("{id}")]
        public IActionResult DeleteEmployeeTaskById(int id)
        {
            var empT = _cxt.EmployeesTasks.Include(p => p.Employee)
                                          .Include(p => p.Task).FirstOrDefault(x => x.Id == id);

            if (empT == null)
            {
                return NotFound();
            }
            _cxt.EmployeesTasks.Remove(empT);
            _cxt.SaveChanges();
            return Ok(empT);
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
        public IActionResult UpdateCustomer(int id, [FromBody] Employee values)
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








        //========================================================================================================================
        //EMPTASK ENDPOINTS

        [HttpGet("Task")]
        public IActionResult GetTask()
        {
            var task = _cxt.EmpTasks.ToList();

            if (task == null)
            {
                return BadRequest();
            }
            return Ok(task);

        }


        //Finds Individual Record By Id
        [HttpGet("Task/{Id}")]
        public IActionResult GetTaskById(int id)
        {
            var task = _cxt.EmpTasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }


        //Create Record
        [HttpPost("TaskPost")]
        public IActionResult CreateTask([FromBody] EmpTask values)
        {
            _cxt.EmpTasks.Add(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetTaskById), new { id = values.Id }, values);
        }


        //Edit Record
        [HttpPut("TaskPut")]
        public IActionResult UpdateTask(int id, [FromBody] EmpTask values)
        {
            var task = _cxt.EmpTasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }
            _cxt.EmpTasks.Update(values);
            _cxt.SaveChanges();
            return CreatedAtAction(nameof(GetTaskById), new { id = values.Id }, values);
        }


    }
}
