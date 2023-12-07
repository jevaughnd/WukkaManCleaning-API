using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WukkaMan_Interface.Models;
using WukkaMan_Interface.Models.ViewModel;

namespace WukkaMan_Interface.Controllers
{
    public class TaskController : Controller
    {

        const string BASE_URL = "https://localhost:7116/api/EmployeeTaskAPI";

        const string EMPLOYEE_ENDPOINT = "Employee";

        const string EMPTASK_ENDPOINT = "Task";


        //INDEX
        public IActionResult Index()
        {
            //Succes Message
            if (TempData.ContainsKey("message"))
            {
                ViewData["message"] = TempData["message"].ToString();
            }//-------------


            var empTaskList = new List<EmployeeTask>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                
                HttpResponseMessage response = client.GetAsync($"{BASE_URL}/EmployeeTask").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    empTaskList = JsonConvert.DeserializeObject<List<EmployeeTask>>(data);
                }
            }

            return View(empTaskList);
        }



        //CREATE:GET
        [HttpGet]
        public IActionResult Create()
        {
            EmployeeTask employeeTask = new EmployeeTask();

            List<Employee> employeeList = new List<Employee>();
            List<EmpTask> empTaskList = new List<EmpTask>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}"); ///
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                //employee
                HttpResponseMessage empResponse = client.GetAsync($"{BASE_URL}/{EMPLOYEE_ENDPOINT}").Result;
                if (empResponse.IsSuccessStatusCode)
                {
                    var empData = empResponse.Content.ReadAsStringAsync().Result;
                    employeeList = JsonConvert.DeserializeObject<List<Employee>>(empData)!;
                }


                //task
                HttpResponseMessage taskResponse = client.GetAsync($"{BASE_URL}/{EMPTASK_ENDPOINT}").Result;
                if (taskResponse.IsSuccessStatusCode)
                {
                    var taskData = taskResponse.Content.ReadAsStringAsync().Result;
                    empTaskList = JsonConvert.DeserializeObject<List<EmpTask>>(taskData)!;
                }


                //DDL in EmployeeVM
                var viewModel = new TaskVM
                {
                    //employee
                    EmployeeList = employeeList.Select(emp => new SelectListItem
                    {
                        Text = emp.FullName,
                        Value = emp.Id.ToString(),
                    }).ToList(),

                    //task
                    TaskList = empTaskList.Select(etsk => new SelectListItem
                    {
                        Text = etsk.TaskName, Value = etsk.Id.ToString(),
                    }).ToList()
                };
                return View(viewModel);
            }
        }



        //CREATE:POST
        [HttpPost]
        public IActionResult Create(TaskVM taskVm, int id)
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{BASE_URL}"); ///
            client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var employeeTask = new EmployeeTask
            {
                Id = id,
                EmployeeId  = taskVm.SelectedEployeeId,
                TaskId = taskVm.SelectedTaskId,
                isCompleted = taskVm.isCompleted,
                AssigmentDate = taskVm.AssigmentDate,
               
            };


            var json = JsonConvert.SerializeObject(employeeTask);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage empTaskResponse = client.PutAsync($"{BASE_URL}/EmployeeTaskPut", data).Result; //Updates use put request 
            if (empTaskResponse.IsSuccessStatusCode)
            {
                TempData["message"] = "Task Added Successfully"; // Succes message
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Unable to create Task");
                return View(taskVm);
            }

        }



        //--------------------------------------------------------------------
        //EDIT:GET

        [HttpGet]
        public IActionResult Edit(int id)
        {
            EmployeeTask employeeTask = new EmployeeTask();//global

            List<Employee> employeeList = new List<Employee>();
            List<EmpTask> empTaskList = new List<EmpTask>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}"); ///
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                //employee
                HttpResponseMessage empResponse = client.GetAsync($"{BASE_URL}/{EMPLOYEE_ENDPOINT}").Result;
                if (empResponse.IsSuccessStatusCode)
                {
                    var empData = empResponse.Content.ReadAsStringAsync().Result;
                    employeeList = JsonConvert.DeserializeObject<List<Employee>>(empData)!;
                }


                //task
                HttpResponseMessage taskResponse = client.GetAsync($"{BASE_URL}/{EMPTASK_ENDPOINT}").Result;
                if (taskResponse.IsSuccessStatusCode)
                {
                    var taskData = taskResponse.Content.ReadAsStringAsync().Result;
                    empTaskList = JsonConvert.DeserializeObject<List<EmpTask>>(taskData)!;
                }



                //----------------------------------------------------------------------
                //get emp tasks by {id} to show values in form
                HttpResponseMessage empTaskRes = client.GetAsync($"{BASE_URL}/{id}").Result;
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



        //EDIT:POST
        [HttpPost]
        public IActionResult Edit(TaskVM taskVm)
        {





            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BASE_URL); ///
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

            HttpResponseMessage empTaskResponse = client.PutAsync($"{BASE_URL}/EmployeeTaskPut", data).Result; //Updates use put request 
            if (empTaskResponse.IsSuccessStatusCode)
            {
                TempData["message"] = "Task Updated Successfully"; // Succes message
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Unable to Update Task");
                return View(taskVm);
            }

        }











        //------------------------------------------------
        //DETAIL
        public IActionResult Detail(int id)
        {
         
            var empployeeTask = new EmployeeTask();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
               
                HttpResponseMessage response = client.GetAsync($"{BASE_URL}/{id}").Result;
                
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    empployeeTask = JsonConvert.DeserializeObject<EmployeeTask>(data);
                }
            }
            return View(empployeeTask);
        }



        //--------------------------------------------------
        //DELETE:GET
        [HttpGet]
        public IActionResult Delete(int id)
        {

            var empployeeTask = new EmployeeTask();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync($"{BASE_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    empployeeTask = JsonConvert.DeserializeObject<EmployeeTask>(data);
                }
            }
            return View(empployeeTask);
        }



        //DELETE:POST
        [HttpPost]
        public IActionResult Delete(int id, EmployeeTask employeeTask)
        {

            //var empployeeTask = new EmployeeTask();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                
                HttpResponseMessage response = client.DeleteAsync($"{BASE_URL}/{id}").Result; //delete
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    employeeTask = JsonConvert.DeserializeObject<EmployeeTask>(data);
                }
            }
            return RedirectToAction("Index");
        }


    }

}
