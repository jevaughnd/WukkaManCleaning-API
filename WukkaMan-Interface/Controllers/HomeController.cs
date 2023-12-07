using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WukkaMan_Interface.Models;

namespace WukkaMan_Interface.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            //Logged In Succes Message, From Login Controller
            if (TempData.ContainsKey("message"))
            {
                ViewData["message"] = TempData["message"].ToString();
            }//-------------


            


			return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}