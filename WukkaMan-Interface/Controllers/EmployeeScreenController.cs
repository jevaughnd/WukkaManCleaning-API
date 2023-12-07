using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;
using WukkaMan_Interface.Models;
using WukkaMan_Interface.Models.ViewModel;

namespace WukkaMan_Interface.Controllers
{
    public class EmployeeScreenController : Controller

    {
        

        const string EMPLOYEE_ENPOINT = "Employee";

        public IActionResult Index()
        {




            return View();
        }


        //----------------------------------------------------------------------------------------------


        const string EMPLOYEE_ENDPOINT = "Employee";
        const string EMP_API_URL = "https://localhost:7116/api/EmployeeAPI";

        // CREATE: GET - Emp Login
        public IActionResult EmployeeLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EmployeeLogin(Employee employee)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{EMP_API_URL}");
                    client.DefaultRequestHeaders.Accept.Clear();

                    // Gets an employee with the already provided Work ID and Id
                    HttpResponseMessage response = await client.GetAsync($"{EMP_API_URL}/{EMPLOYEE_ENDPOINT}/{employee.WorkId}/{employee.Id}");

                    if (response.IsSuccessStatusCode)
                    {
                        // We Now Deserialize the employee from the response
                        var data = await response.Content.ReadAsStringAsync();
                        Employee loggedInEmployee = JsonConvert.DeserializeObject<Employee>(data);

                        // Check if the entered WorkId and Id match the stored values
                        if (loggedInEmployee != null)
                        {
                            // Log the clock in time
                            DateTime clockInTime = DateTime.Now;

                            // Display the clock in time to the employee
                            TempData["ClockInTime"] = $"You Clocked In: {clockInTime.ToString("yyyy-MM-dd HH:mm:ss")}";

                            return RedirectToAction("EmployeeDetail", loggedInEmployee);
                        }
                    }

                    // If the response is successful but the employee is not found
                    if (response.IsSuccessStatusCode)
                    {
                        ModelState.AddModelError(string.Empty, "Employee not found. Unable to log in.");
                    }
                    else
                    {
                        // Employee with the provided Work ID and Id not found
                        ModelState.AddModelError(string.Empty, "Invalid Work ID or Id. Unable to log in.");
                    }

                    // Return to the login page with error messages
                    return View(employee);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (log it, display an error message, etc.)
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return View(employee);
            }
        }




        //-------------------------------------------------------------------------------------------






        //---------------------------------------------------


        //GET: EMPLOYEE DETAIL
        public IActionResult EmployeeDetail(int id) // This is the first thing an employee sees when logged inc
        {

            //Display Logged, Clock In time from, method above
            if (TempData.ContainsKey("ClockInTime"))
            {
                ViewData["ClockInTime"] = TempData["ClockInTime"].ToString();
            }//-------------


            //-----------
            // Add the server time to ViewData
            ViewData["ServerTime"] = DateTime.Now;

           

            Employee employee = new Employee();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{EMP_API_URL}/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync($"{EMP_API_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    employee = JsonConvert.DeserializeObject<Employee>(data);
                }
            }
            return View(employee);
        }







        //------------------------------------------------
        const string Task_URL = "https://localhost:7116/api/EmployeeTaskAPI";
        //DETAIL
        public IActionResult TaskDetail(int id)
        {

            var empployeeTask = new EmployeeTask();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{Task_URL}/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync($"{Task_URL}/{id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    empployeeTask = JsonConvert.DeserializeObject<EmployeeTask>(data);
                }
            }
            return View(empployeeTask);
        }



        //---------------------------------------------------------------
        //EDIT TASK:GET
        const string EMPTASK_ENDPOINT = "Task";

        [HttpGet]
        public IActionResult Emp_EditTask(int id)
        {
            EmployeeTask employeeTask = new EmployeeTask();//global

            List<Employee> employeeList = new List<Employee>();
            List<EmpTask> empTaskList = new List<EmpTask>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{Task_URL}"); ///
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                //employee
                HttpResponseMessage empResponse = client.GetAsync($"{Task_URL}/{EMPLOYEE_ENDPOINT}").Result;
                if (empResponse.IsSuccessStatusCode)
                {
                    var empData = empResponse.Content.ReadAsStringAsync().Result;
                    employeeList = JsonConvert.DeserializeObject<List<Employee>>(empData)!;
                }


                //task
                HttpResponseMessage taskResponse = client.GetAsync($"{Task_URL}/{EMPTASK_ENDPOINT}").Result;
                if (taskResponse.IsSuccessStatusCode)
                {
                    var taskData = taskResponse.Content.ReadAsStringAsync().Result;
                    empTaskList = JsonConvert.DeserializeObject<List<EmpTask>>(taskData)!;
                }



                //----------------------------------------------------------------------
                //get emp tasks by {id} to show values in form
                HttpResponseMessage empTaskRes = client.GetAsync($"{Task_URL}/{id}").Result;
                if (empTaskRes.IsSuccessStatusCode)
                {
                    var empData = empTaskRes.Content.ReadAsStringAsync().Result;
                    employeeTask = JsonConvert.DeserializeObject<EmployeeTask>(empData)!;
                }

                //DDL in EmployeeVM
                var viewModel = new TaskVM
                {
                    Id = employeeTask.Id,
                    SelectedEployeeId = employeeTask.EmployeeId,
                    SelectedTaskId = employeeTask.TaskId,
                    isCompleted = employeeTask.isCompleted,
                    AssigmentDate = employeeTask.AssigmentDate,



                    //employee
                    EmployeeList = employeeList.Select(emp => new SelectListItem
                    {
                        Text = emp.FullName,
                        Value = emp.Id.ToString(),
                    }).ToList(),

                    //task
                    TaskList = empTaskList.Select(etsk => new SelectListItem
                    {
                        Text = etsk.TaskName,
                        Value = etsk.Id.ToString(),
                    }).ToList()
                };

                return View(viewModel);
            }
        }



        //EDIT TASK :POST
        [HttpPost]
        public IActionResult Emp_EditTask(TaskVM taskVm)
        {

        
            //Succes Message
            if (TempData.ContainsKey("Task-Message"))
            {
                ViewData["Task-Message"] = TempData["Task-Message"].ToString();
            }//-------------


            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(Task_URL); ///
            client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var employeeTask = new EmployeeTask
            {
                Id = taskVm.Id,
                EmployeeId = taskVm.SelectedEployeeId,
                TaskId = taskVm.SelectedTaskId,
                isCompleted = taskVm.isCompleted,
                AssigmentDate = taskVm.AssigmentDate,

            };


            var json = JsonConvert.SerializeObject(employeeTask);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage empTaskResponse = client.PutAsync($"{Task_URL}/EmployeeTaskPut", data).Result; //Updates use put request 
            if (empTaskResponse.IsSuccessStatusCode)
            {
             

                TempData["Task-Message"] = "Task Updated Successfully"; // Succes message displayed
                return View(taskVm);
               
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Unable to Update Task");
                return View(taskVm);
            }

        }













        //--------------------------------------------
        const string Shift_URL = "https://localhost:7116/api/EmpClockShiftAPI";
        //GET: SHIFT DETAIL

        public IActionResult ShiftDetail(int id)
        {

            EmplyeeClockShift empClockShift = new EmplyeeClockShift();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{Shift_URL}/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync($"{Shift_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    empClockShift = JsonConvert.DeserializeObject<EmplyeeClockShift>(data);
                }
            }
            return View(empClockShift);
        }











        // EMPLOYEE UPDATE LAST SHIFT ----------------
        //-----------------------------------------------------------------------

        //UPDATE:GET
        [HttpGet]
        public IActionResult Update_YourShift(int id)
        {
            EmplyeeClockShift empClockShift = new EmplyeeClockShift();
            List<Employee> empList = new List<Employee>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{Shift_URL}"); ///
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                //employee ddl
                HttpResponseMessage empResponse = client.GetAsync($"{Shift_URL}/{EMPLOYEE_ENDPOINT}").Result;
                if (empResponse.IsSuccessStatusCode)
                {
                    var empData = empResponse.Content.ReadAsStringAsync().Result;
                    empList = JsonConvert.DeserializeObject<List<Employee>>(empData)!;
                }

                //------------------------
                //all EmplyeeClockShift  id
                HttpResponseMessage empClockRes = client.GetAsync($"{Shift_URL}/{id}").Result; // {id} shows values in the form
                if (empClockRes.IsSuccessStatusCode)
                {
                    var empCdata = empClockRes.Content.ReadAsStringAsync().Result;
                    empClockShift = JsonConvert.DeserializeObject<EmplyeeClockShift>(empCdata)!;
                }


                var viewModel = new ClockShiftVM
                {
                    //employee ddl
                    EmployeeList = empList.Select(emp => new SelectListItem
                    {
                        Text = emp.FullName,
                        Value = emp.Id.ToString(),
                    }).ToList(),


                    Id = empClockShift.Id,
                    SelectedEployeeId = empClockShift.EmployeeId,
                    WorkId = empClockShift.WorkId,
                    ClockIn = empClockShift.ClockIn,
                    ClockOut = empClockShift.ClockOut,
                    ShiftDate = empClockShift.ShiftDate,

                };
                return View(viewModel);
            }
        }


        //UPDATE:POST
        [HttpPost]
        public IActionResult Update_YourShift(ClockShiftVM clockShiftvM)
        {
            //Succes Message
            if (TempData.ContainsKey("shift-message"))
            {
                ViewData["shift-message"] = TempData["shift-message"].ToString();
            }//-------------


            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{Shift_URL}"); ///
            client.DefaultRequestHeaders.Accept.Clear();


            var emplyeeClockShift = new EmplyeeClockShift
            {
                Id = clockShiftvM.Id,
                EmployeeId = clockShiftvM.SelectedEployeeId,
                WorkId = clockShiftvM.WorkId,
                ClockIn = clockShiftvM.ClockIn,
                ClockOut = clockShiftvM.ClockOut,
                ShiftDate = clockShiftvM.ShiftDate,
            };


            var json = JsonConvert.SerializeObject(emplyeeClockShift);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage empClockResponse = client.PutAsync($"{Shift_URL}/ClockShiftPut", data).Result; //Updates use put request 
            if (empClockResponse.IsSuccessStatusCode)
            {
                TempData["shift-message"] = "Shift Updated Successfully"; // Succes message displayed
                return View(clockShiftvM);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Unable to Update Clock Shift");
                return View(clockShiftvM);
            }

        }







    }
}
