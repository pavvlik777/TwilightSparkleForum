using Microsoft.AspNetCore.Mvc;

namespace TwilightSparkle.Forum.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}