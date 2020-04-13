using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

using TwilightSparkle.Forum.Foundation.Authentication;
using TwilightSparkle.Forum.Models.Authentication;

using IAuthenticationService = TwilightSparkle.Forum.Foundation.Authentication.IAuthenticationService;

namespace TwilightSparkle.Forum.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;


        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }


        public IActionResult SignIn()
        {
            return View(new SignInViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _authenticationService.SignInAsync(model.Username, model.Password, false, Authenticate);
            if (!result.IsSuccess)
            {
                var errorMessage = GetErrorMessage(result.ErrorType);
                ModelState.AddModelError("", errorMessage);

                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }


        private async Task Authenticate(string username, int userId, bool rememberMe)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, username),
                new Claim("UserId", userId.ToString())
            };
            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            var authProperties = new AuthenticationProperties();
            if (rememberMe)
            {
                authProperties.IsPersistent = true;
                authProperties.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30);
            }
            else
            {
                authProperties.IsPersistent = false;
            }

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id), authProperties);
        }

        private static string GetErrorMessage(SignInErrorType error)
        {
            switch (error)
            {
                case SignInErrorType.InvalidCredentials:
                    return "Invalid credentials";
                default:
                    throw new ArgumentOutOfRangeException(nameof(error), error, null);
            }
        }
    }
}