using Microsoft.AspNetCore.Mvc;

namespace TwilightSparkle.Forum.Controllers.Api
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}