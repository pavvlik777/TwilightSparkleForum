using System;

using Microsoft.AspNetCore.Mvc;

namespace TwilightSparkle.Forum.IdentityExtensions
{
    public static class ProfileImageExtension
    {
        public static string GetProfileImageUrl(this IUrlHelper urlHelper)
        {
            var identity = urlHelper.ActionContext.HttpContext.User.Identity;
            if (!identity.IsAuthenticated)
            {
                throw new ArgumentException("Must be authenticated");
            }

            var imageUrl = urlHelper.Action("GetForCurrentUser", "Images", null, urlHelper.ActionContext.HttpContext.Request.Scheme);

            return imageUrl;
        }
    }
}
