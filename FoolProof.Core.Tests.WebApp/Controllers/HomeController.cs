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

        [HttpGet("eq2")]
        public IActionResult EqualTo([FromQuery] bool pwn = false)
        {
            object model = pwn ? new EqualTo.ModelWithPassOnNull() : new EqualTo.Model();
            ViewBag.PassWithNull = pwn;
            return View("EqualTo", model);
        }

        [HttpGet("neq2")]
        public IActionResult NotEqualTo([FromQuery] bool pwn = false)
        {
            object model = pwn ? new NotEqualTo.ModelWithPassOnNull() : new NotEqualTo.Model();
            ViewBag.PassWithNull = pwn;
            return View("NotEqualTo", model);
        }

        [HttpGet("gt/{type}")]
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

        [HttpGet("lt/{type}")]
        public IActionResult LessThan([FromRoute] string type, [FromQuery] bool pwn = false)
        {
            object model = type.ToLowerInvariant() switch
            {
                "date" => pwn ? new LessThan.DateModelWithPassOnNull() : new LessThan.DateModel(),
                "int16" => pwn ? new LessThan.Int16ModelWithPassOnNull() : new LessThan.Int16Model(),
                "time" => pwn ? new LessThan.TimeModelWithPassOnNull() : new LessThan.TimeModel(),
                _ => throw new HttpRequestException("Unsupported data type", null, System.Net.HttpStatusCode.BadRequest)
            };
            ViewBag.DataType = type;
            ViewBag.PassWithNull = pwn;
            return View("LessThan", model);
        }

        [HttpGet("ge2/{type}")]
        public IActionResult GreaterOrEqualTo([FromRoute] string type, [FromQuery] bool pwn = false)
        {
            object model = type.ToLowerInvariant() switch
            {
                "date" => pwn ? new GreaterThanOrEqualTo.DateModelWithPassNull() : new GreaterThanOrEqualTo.DateModel(),
                "int16" => pwn ? new GreaterThanOrEqualTo.DateModelWithPassNull() : new GreaterThanOrEqualTo.Int16Model(),
                "time" => pwn ? new GreaterThanOrEqualTo.DateModelWithPassNull() : new GreaterThanOrEqualTo.TimeModel(),
                _ => throw new HttpRequestException("Unsupported data type", null, System.Net.HttpStatusCode.BadRequest)
            };
            ViewBag.DataType = type;
            ViewBag.PassWithNull = pwn;
            return View("GreaterOrEqualTo", model);
        }

        [HttpGet("le2/{type}")]
        public IActionResult LessOrEqualTo([FromRoute] string type, [FromQuery] bool pwn = false)
        {
            object model = type.ToLowerInvariant() switch
            {
                "date" => pwn ? new LessThanOrEqualTo.DateModelWithPassNull() : new LessThanOrEqualTo.DateModel(),
                "int16" => pwn ? new LessThanOrEqualTo.DateModelWithPassNull() : new LessThanOrEqualTo.Int16Model(),
                "time" => pwn ? new LessThanOrEqualTo.DateModelWithPassNull() : new LessThanOrEqualTo.TimeModel(),
                _ => throw new HttpRequestException("Unsupported data type", null, System.Net.HttpStatusCode.BadRequest)
            };
            ViewBag.DataType = type;
            ViewBag.PassWithNull = pwn;
            return View("LessOrEqualTo", model);
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

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            ViewBag.UseInputTypes = Request.Cookies.TryGetValue("UseInputTypes", out var cookie)
                                    ? bool.TryParse(cookie, out var useInputTypes)
                                        && useInputTypes
                                    : (bool?)null;
        }
    }
}