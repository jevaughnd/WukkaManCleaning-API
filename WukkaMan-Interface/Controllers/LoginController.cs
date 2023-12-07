using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WukkaMan_Interface.Models;

namespace WukkaMan_Interface.Controllers
{
    public class LoginController : Controller
    {
        const string AUTH_URL = "https://localhost:7116/api/AuthAPI";

        const string SESSION_AUTH = "WukkaManCleaning-API"; //name of project

        const string Emp_Url = "https://localhost:7116/api/EmployeeAPI";



        [HttpGet]
        public async Task<IActionResult> Index()
        {

            //Retrieve JWT token from local storage
            string token = RetrieveTokenFromLocalStorage();
            if (string.IsNullOrEmpty(token))
            {
                //if the token is missing or expired
                return RedirectToAction("Login");
            }
              
            //To See Employee List
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Emp_Url);

                //Add the JWT token to the Authorization header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Make a request to the Employees API to retieve the list of employees
                HttpResponseMessage response = client.GetAsync($"{Emp_Url}/Employee").Result;
                if (response.IsSuccessStatusCode)
                {
                    ///Parse and deserialize the response content to a list of employees
                    var data = response.Content.ReadAsStringAsync().Result;
                    ///Deserialise object from Json string
                    var employeeList = JsonConvert.DeserializeObject<List<Employee>>(data);

                    return View(employeeList);
                }
                else
                {
                    return View("Error");
                }
            }
        }



        [HttpGet] //get Login Page
        public IActionResult Login()
        {
            return View();
        }


 

       

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            if (ModelState.IsValid)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(AUTH_URL);
                    string jsonContent = JsonConvert.SerializeObject(user);

                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");


                    //send login to API
                    HttpResponseMessage response = await client.PostAsync($"{AUTH_URL}/login", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent);

                        if (responseData.ContainsKey("status") && responseData["status"].ToString() == "succes")
                        {
                            if (responseData.ContainsKey("data"))
                            {

                                var token = responseData["data"].ToString();
                                HttpContext.Session.SetString(SESSION_AUTH, token);

                            }
							TempData["message"] = "Successfully Logged In"; // Succes message displayed in Home Index
                            return RedirectToAction("Index", "Employee");

						
						}
                        else
                        {
                            //login faild
                            ModelState.AddModelError(string.Empty, "Invalid Username Or Password");
                            return View(user);
                        }
                    }
                    else
                    {
                        //login faild
                        ModelState.AddModelError(string.Empty, "Invalid Username Or Password");
                        return View(user);
                    }
                }
            }
            return View(user);
        }



        // Reference / called in index 
        private string RetrieveTokenFromLocalStorage()
        {
            string token = HttpContext.Session.GetString(SESSION_AUTH)!;
            return token;
        }
    }
}
