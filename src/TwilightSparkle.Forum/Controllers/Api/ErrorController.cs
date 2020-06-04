using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using TwilightSparkle.Forum.ControllerExtenstions;

namespace TwilightSparkle.Forum.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : Controller
    {
        [Route("NotFound")]
        public async Task<IActionResult> NotFoundError()
        {
            var content = await this.RenderViewToStringAsync("/Views/Error/NotFoundError.cshtml");

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = content
            };
        }

        [Route("InternalError")]
        public async Task<IActionResult> InternalError()
        {
            var content = await this.RenderViewToStringAsync("/Views/Error/InternalError.cshtml");

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = content
            };
        }
    }
}