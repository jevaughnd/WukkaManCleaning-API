using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using WukkaMan_Interface.Models;



namespace WukkaMan_Interface.Controllers
{
    public class EmployeeController : Controller
    {

        const string BASE_URL = "https://localhost:7116/api/EmployeeAPI";

        const string EMPLOYEE_ENPOINT = "Employee";


      
        //INDEX
        public IActionResult Index()
        {

            //Succes Message
            if (TempData.ContainsKey("message"))
            {
                ViewData["message"] = TempData["message"].ToString();
            }//-----------------------------------------------------
            


            var employeeList = new List<Employee>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync($"{BASE_URL}/Employee").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    employeeList = JsonConvert.DeserializeObject<List<Employee>>(data);
                }
            }

            return View(employeeList);
        }


        //CREATE:GET
        public IActionResult Create()
        {
            Employee employee = new Employee();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}"); ///
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                
                return View(employee);
            }
        }



        //CREATE:POST
        [HttpPost]
        public IActionResult Create(Employee employee, int id)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{BASE_URL}"); ///
            client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var emp = new Employee
            {
                Id = id,
                FullName = employee.FullName,
                EmailAddress = employee.EmailAddress,
                PhoneNumber =  employee.PhoneNumber,
                WorkId = employee.WorkId,
            };


            var json = JsonConvert.SerializeObject(employee);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage empResponse = client.PutAsync($"{BASE_URL}/EmployeePut", data).Result; //Updates use put request 
            if (empResponse.IsSuccessStatusCode)
            {


                TempData["message"] = "Employee Added Successfully"; // Succes message displayed in index
                return RedirectToAction("Index"); 
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Unable to create Employee");
                return View(employee);
            }
        }



        //---------------------------------------------------
        //EDIT:GET
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Employee employee = new Employee();

            using (HttpClient client = new HttpClient()) 
            {

                client.BaseAddress = new Uri($"{BASE_URL}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage empResponse = client.GetAsync($"{BASE_URL}/{id}").Result; // {id} shows values
                if (empResponse.IsSuccessStatusCode)
                {
                    var data = empResponse.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    employee = JsonConvert.DeserializeObject<Employee>(data)!;
                }

                var employee1 = new Employee
                {
                    Id = id,
                    FullName = employee.FullName,
                    EmailAddress = employee.EmailAddress,
                    PhoneNumber = employee.PhoneNumber,
                    WorkId = employee.WorkId,
                };
            }
            return View(employee);
        }



        //EDIT:POST
        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            //Employee employee = new Employee();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{BASE_URL}");
            client.DefaultRequestHeaders.Accept.Clear();
               


                var emp = new Employee
                {
                    Id = employee.Id,
                    FullName = employee.FullName,
                    EmailAddress = employee.EmailAddress,
                    PhoneNumber = employee.PhoneNumber,
                    WorkId = employee.WorkId,
                };

            var json = JsonConvert.SerializeObject(emp);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage empResponse = client.PutAsync($"{BASE_URL}/EmployeePut", data).Result; //Updates use put request 
            if (empResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Sorry, Unable to Update Employee");
                return View(employee);
            }
        }


        //----------------------------------------------------------------
        //DETAIL
        public IActionResult Detail(int id)
        {
            Employee employee = new Employee();

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
                    employee = JsonConvert.DeserializeObject<Employee>(data);
                }
            }
            return View(employee);
        }



        //--------------------------------------------------------
        //DELETE: GET

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Employee employee = new Employee();

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
                    employee = JsonConvert.DeserializeObject<Employee>(data);
                }
            }
            return View(employee);
        }


        //DELETE:POST
        [HttpPost]
        public IActionResult Delete(int id, Employee employee)
        {
            //Employee employee = new Employee();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.DeleteAsync($"{BASE_URL}/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    employee = JsonConvert.DeserializeObject<Employee>(data);
                }
            }
            return RedirectToAction("Index");
        }

    }
}
