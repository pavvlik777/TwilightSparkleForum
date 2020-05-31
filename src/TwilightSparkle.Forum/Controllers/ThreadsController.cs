using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TwilightSparkle.Forum.Controllers
{
    [Authorize]
    public class ThreadsController : Controller
    {
        public IActionResult CreateThread()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateThread(int i) //TODO
        {
            return View();
        }
    }
}