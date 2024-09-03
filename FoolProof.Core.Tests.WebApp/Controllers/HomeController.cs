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
        public IActionResult EqualTo([FromQuery] bool pwn = false)
        {
            object model = pwn ? new EqualTo.ModelWithPassOnNull() : new EqualTo.Model();
            ViewBag.PassWithNull = pwn;
            return View("EqualTo", model);
        }

        [HttpGet("notequalto")]
        public IActionResult NotEqualTo([FromQuery] bool pwn = false)
        {
            object model = pwn ? new NotEqualTo.ModelWithPassOnNull() : new NotEqualTo.Model();
            ViewBag.PassWithNull = pwn;
            return View("NotEqualTo", model);
        }

        [HttpGet("greaterthan/{type}")]
        public IActionResult GreaterThan([FromRoute] string type, [FromQuery] bool pwn = false)
        {
            object model = type.ToLowerInvariant() switch
            {
                "date" => pwn ? new GreaterThan.DateModelWithPassOnNull() : new GreaterThan.DateModel(),
                "int16" => pwn ? new GreaterThan.Int16ModelWithPassOnNull() : new GreaterThan.Int16Model(),
                "time" => pwn ? new GreaterThan.TimeModelWithPassOnNull() : new GreaterThan.TimeModel(),
                _ => throw new HttpRequestException("Unsupported data type", null, System.Net.HttpStatusCode.BadRequest)
            };
            ViewBag.DataType = type;
            ViewBag.PassWithNull = pwn;
            return View("GreaterThan", model);
        }

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