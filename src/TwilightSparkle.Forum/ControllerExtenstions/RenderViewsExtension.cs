using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace TwilightSparkle.Forum.ControllerExtenstions
{
    public static class RenderViewsExtension
    {
        public static async Task<string> RenderViewToStringAsync<TModel>(this Controller controller, string viewNamePath, TModel model)
        {
            if (string.IsNullOrEmpty(viewNamePath))
            {
                viewNamePath = controller.ControllerContext.ActionDescriptor.ActionName;
            }

            controller.ViewData.Model = model;

            using var writer = new StringWriter();
            try
            {
                var viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;

                ViewEngineResult viewResult = null;

                if (viewNamePath.EndsWith(".cshtml"))
                {
                    viewResult = viewEngine.GetView(viewNamePath, viewNamePath, false);
                }
                else
                {
                    viewResult = viewEngine.FindView(controller.ControllerContext, viewNamePath, false);
                }

                if (!viewResult.Success)
                {
                    return $"A view with the name '{viewNamePath}' could not be found";
                }

                var viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
            catch (Exception ex)
            {
                return $"Failed - {ex.Message}";
            }
        }

        public static async Task<string> RenderViewToStringAsync(this Controller controller, string viewNamePath)
        {
            if (string.IsNullOrEmpty(viewNamePath))
            {
                viewNamePath = controller.ControllerContext.ActionDescriptor.ActionName;
            }

            using var writer = new StringWriter();
            try
            {
                var viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;

                ViewEngineResult viewResult = null;

                if (viewNamePath.EndsWith(".cshtml"))
                {
                    viewResult = viewEngine.GetView(viewNamePath, viewNamePath, false);
                }
                else
                {
                    viewResult = viewEngine.FindView(controller.ControllerContext, viewNamePath, false);
                }

                if (!viewResult.Success)
                {
                    return $"A view with the name '{viewNamePath}' could not be found";
                }

                var viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
            catch (Exception ex)
            {
                return $"Failed - {ex.Message}";
            }
        }
    }
}
