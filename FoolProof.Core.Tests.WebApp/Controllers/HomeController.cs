using FoolProof.Core.Tests.E2eTests.WebApp.Models;
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
        public IActionResult EqualTo()
        {
            object model = new EqualTo.Model();
            return View("EqualTo", model);
        }

        [HttpGet("neq2")]
        public IActionResult NotEqualTo()
        {
            object model = new NotEqualTo.Model();
            return View("NotEqualTo", model);
        }

        [HttpGet("gt/{type}")]
        public IActionResult GreaterThan([FromRoute] string type)
        {
            object model = type.ToLowerInvariant() switch
            {
                "date" => new GreaterThan.DateModel(),
                "int16" => new GreaterThan.Int16Model(),
                "time" => new GreaterThan.TimeModel(),
                "datetime" => new GreaterThan.DateTimeModel(),
                _ => throw new HttpRequestException("Unsupported data type", null, System.Net.HttpStatusCode.BadRequest)
            };
            ViewBag.DataType = type;
            return View("GreaterThan", model);
        }

        [HttpGet("lt/{type}")]
        public IActionResult LessThan([FromRoute] string type)
        {
            object model = type.ToLowerInvariant() switch
            {
                "date" => new LessThan.DateModel(),
                "int16" => new LessThan.Int16Model(),
                "time" => new LessThan.TimeModel(),
                "datetime" => new LessThan.DateTimeModel(),
                _ => throw new HttpRequestException("Unsupported data type", null, System.Net.HttpStatusCode.BadRequest)
            };
            ViewBag.DataType = type;
            return View("LessThan", model);
        }

        [HttpGet("ge2/{type}")]
        public IActionResult GreaterOrEqualTo([FromRoute] string type)
        {
            object model = type.ToLowerInvariant() switch
            {
                "date" => new GreaterThanOrEqualTo.DateModel(),
                "int16" => new GreaterThanOrEqualTo.Int16Model(),
                "time" => new GreaterThanOrEqualTo.TimeModel(),
                "datetime" => new GreaterThanOrEqualTo.DateTimeModel(),
                _ => throw new HttpRequestException("Unsupported data type", null, System.Net.HttpStatusCode.BadRequest)
            };
            ViewBag.DataType = type;
            return View("GreaterOrEqualTo", model);
        }

        [HttpGet("le2/{type}")]
        public IActionResult LessOrEqualTo([FromRoute] string type)
        {
            object model = type.ToLowerInvariant() switch
            {
                "date" => new LessThanOrEqualTo.DateModel(),
                "int16" => new LessThanOrEqualTo.Int16Model(),
                "time" => new LessThanOrEqualTo.TimeModel(),
                "datetime" => new LessThanOrEqualTo.DateTimeModel(),
                _ => throw new HttpRequestException("Unsupported data type", null, System.Net.HttpStatusCode.BadRequest)
            };
            ViewBag.DataType = type;
            return View("LessOrEqualTo", model);
        }

        [HttpGet("in/{type}")]
        public IActionResult In([FromRoute] string type)
        {
            object model = type.ToLowerInvariant() switch
            {
                "single" => new In.SingleValueModel<string>(),
                "datetime" => new In.DateTimeListModel(),
                "int16" => new In.In16ListModel(),
                _ => throw new HttpRequestException("Unsupported data type", null, System.Net.HttpStatusCode.BadRequest)
            };
            ViewBag.DataType = type;
            ViewBag.MultiSelect = type.ToLowerInvariant() != "single";
            return View("In", model);
        }

        [HttpGet("notin/{type}")]
        public IActionResult NotIn([FromRoute] string type)
        {
            object model = type.ToLowerInvariant() switch
            {
                "single" => new NotIn.SingleValueModel<string>(),
                "datetime" => new NotIn.DateTimeListModel(),
                "int16" => new NotIn.In16ListModel(),
                _ => throw new HttpRequestException("Unsupported data type", null, System.Net.HttpStatusCode.BadRequest)
            };
            ViewBag.DataType = type;
            ViewBag.MultiSelect = type.ToLowerInvariant() != "single";
            return View("NotIn", model);
		}

		[HttpGet("complexmodel")]
        public IActionResult ComplexModel()
        {
            return View("ComplexModel", new PersonalInfo());
        }

		[HttpPost("complexmodel")]
		public IActionResult ComplexModel(PersonalInfo? vm)
		{
            if (ModelState.IsValid)
            {
                ViewBag.SuccessMessage = "Validation succeeded";
			}

			return View("ComplexModel", vm);
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

            ViewBag.UseInputTypes = Request.Query.TryGetValue("__useInputTypes__", out var vals)
                                    && bool.TryParse(vals.FirstOrDefault(), out var useInputTypes)
                                    ? useInputTypes
                                    : Request.Cookies.TryGetValue("UseInputTypes", out var cookie)
                                      && bool.TryParse(cookie, out useInputTypes)
                                      ? useInputTypes
                                      : (bool?)null;
        }
    }
}