using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using WukkaMan_Interface.Models;
using WukkaMan_Interface.Models.ViewModel;

namespace WukkaMan_Interface.Controllers
{
    public class ClockShiftController : Controller
    {
        const string BASE_URL = "https://localhost:7116/api/EmpClockShiftAPI";
        const string EMPLOYEE_ENDPOINT = "Employee";
        public IActionResult Index()
        {
            //Succes Message
            if (TempData.ContainsKey("message"))
            {
                ViewData["message"] = TempData["message"].ToString();
            }//-----------------------------------------------------


            var empClockShift = new List<EmplyeeClockShift>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync($"{BASE_URL}/ClockShift").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;

                    //Deserialise object from Json string
                    empClockShift = JsonConvert.DeserializeObject<List<EmplyeeClockShift>>(data);
                }
            }
            return View(empClockShift);
        }


        
        //CREATE:GET
        [HttpGet]
        public IActionResult Create()
        {
            EmplyeeClockShift empClockShift = new EmplyeeClockShift();
            List<Employee> empList = new List<Employee>();

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
                    empList = JsonConvert.DeserializeObject<List<Employee>>(empData)!;
                }


                var viewModel = new ClockShiftVM
                {
                    //employee
                    EmployeeList= empList.Select(emp => new SelectListItem
                    {
                        Text = emp.FullName,
                        Value = emp.Id.ToString(),
                    }).ToList(),
                };
             
                return View(viewModel);
            }
        }





        //CREATE:POST
        [HttpPost]
        public IActionResult Create(ClockShiftVM clockShiftvM, int id)
        {


            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{BASE_URL}"); ///
            client.DefaultRequestHeaders.Accept.Clear();


            var emplyeeClockShift = new EmplyeeClockShift
            {
                Id = id,

                EmployeeId = clockShiftvM.SelectedEployeeId,
                WorkId = clockShiftvM.WorkId,
                ClockIn = clockShiftvM.ClockIn,
                ClockOut = clockShiftvM.ClockOut,
                ShiftDate = clockShiftvM.ShiftDate,
             

            };


            var json = JsonConvert.SerializeObject(emplyeeClockShift);
            var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage empClockResponse = client.PutAsync($"{BASE_URL}/ClockShiftPut", data).Result; //Updates use put request 
            if (empClockResponse.IsSuccessStatusCode)
            {
                TempData["message"] = "Shift Added Successfully"; // Succes message displayed in index
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Unable to create Clock Shift");
                return View(clockShiftvM);
            }

        }



         //-----------------------------------------------------------------------

        //EDIT:GET
        [HttpGet]
        public IActionResult EDIT(int id)
        {
            EmplyeeClockShift empClockShift = new EmplyeeClockShift();
            List<Employee> empList = new List<Employee>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{BASE_URL}"); ///
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                //employee ddl
                HttpResponseMessage empResponse = client.GetAsync($"{BASE_URL}/{EMPLOYEE_ENDPOINT}").Result;
                if (empResponse.IsSuccessStatusCode)
                {
                    var empData = empResponse.Content.ReadAsStringAsync().Result;
                    empList = JsonConvert.DeserializeObject<List<Employee>>(empData)!;
                }

                //------------------------
                //all EmplyeeClockShift  id
                HttpResponseMessage empClockRes = client.GetAsync($"{BASE_URL}/{id}").Result; // {id} shows values in the form
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


        //EDIT:POST
        [HttpPost]
        public IActionResult EDIT(ClockShiftVM clockShiftvM)
        {


            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{BASE_URL}"); ///
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

            HttpResponseMessage empClockResponse = client.PutAsync($"{BASE_URL}/ClockShiftPut", data).Result; //Updates use put request 
            if (empClockResponse.IsSuccessStatusCode)
            {
                TempData["message"] = "Shift Updated Successfully"; // Succes message displayed in index
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Unable to Update Clock Shift");
                return View(clockShiftvM);
            }

        }



        //--------------------------------------------
        //DETAIL:GET

        public IActionResult Detail(int id)
        {
            
            EmplyeeClockShift empClockShift = new EmplyeeClockShift();

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
                    empClockShift = JsonConvert.DeserializeObject<EmplyeeClockShift>(data);
                }
            }
            return View(empClockShift);
        }





        //DELETE:GET
        [HttpGet]
        public IActionResult DELETE(int id)
        {

            EmplyeeClockShift empClockShift = new EmplyeeClockShift();

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
                    empClockShift = JsonConvert.DeserializeObject<EmplyeeClockShift>(data);
                }
            }
            return View(empClockShift);
        }


        //DELETE:POST
        [HttpPost]

        public IActionResult DELETE(int id, EmplyeeClockShift empClockShift)
        {

            //EmplyeeClockShift empClockShift = new EmplyeeClockShift();

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
                    empClockShift = JsonConvert.DeserializeObject<EmplyeeClockShift>(data);
                }
            }
            TempData["message"] = "Shift Deleted Successfully"; // Succes message displayed in index
            return RedirectToAction("Index");
        }
    }
}
