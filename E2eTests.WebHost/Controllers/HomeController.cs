using System.Diagnostics;
using E2eTests.WebHost.Models;
using Microsoft.AspNetCore.Mvc;

namespace E2eTests.WebHost.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [TempData]
        public string SuccessMessage { get; set; }

        public IActionResult Index()
        {
            this.ViewBag.SuccessMessage = SuccessMessage;
            return View(new PersonalInfoViewModel());
        }

        [HttpPost]
        public IActionResult Save(PersonalInfoViewModel model)
        {
            if(ModelState.IsValid) 
            {
                this.SuccessMessage = "Model was successfully validated.";
                return RedirectToAction("Index");
            }

            return View("Index", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
