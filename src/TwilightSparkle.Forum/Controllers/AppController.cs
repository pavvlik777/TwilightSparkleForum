using Microsoft.AspNetCore.Mvc;

namespace TwilightSparkle.Forum.Controllers
{
    public class AppController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}