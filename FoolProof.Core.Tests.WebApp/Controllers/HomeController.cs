using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FoolProof.Core.Tests.E2eTests.WebApp.Controllers
{
    [AllowAnonymous]
    public class HomeController(ILogger<HomeController> logger) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;

        public IActionResult Index() => View();

        [HttpGet("equalto")]
        public IActionResult EqualTo() => View("/Views/EqualTo/Index.cshtml");

        [HttpGet("equalto-pwn")]
        public IActionResult EqualToWithNull() => View("/Views/EqualTo/PassWithNull.cshtml");

        [HttpPost("validate")]
        public async Task<JsonResult> Save([FromQuery]string modelTypeName)
        {
            var modelType = typeof(EqualTo).Assembly.GetType(modelTypeName)
                            ?? throw new HttpRequestException($"Couldn't find type info: {modelTypeName}", null, System.Net.HttpStatusCode.FailedDependency);

            var model = Activator.CreateInstance(modelType)
                        ?? throw new HttpRequestException($"Couldn't create and instance of model type: {modelTypeName}", null, System.Net.HttpStatusCode.FailedDependency);

            var updated = await TryUpdateModelAsync(model, modelType!, "");
            return Json(new
            {
                Succeed = updated && ModelState.IsValid,
                Errors = ModelState.Values.SelectMany(mv => mv.Errors).Select(err => err.ErrorMessage)
            });
        }
    }
}