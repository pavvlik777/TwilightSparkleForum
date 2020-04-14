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


        public IActionResult SignUp()
        {
            return View(new SignUpViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var signUpDto = CreateDto(model);
            var result = await _authenticationService.SignUpAsync(signUpDto);
            if (!result.IsSuccess)
            {
                var errorMessage = GetErrorMessage(result.ErrorType);
                ModelState.AddModelError("", errorMessage);

                return View(model);
            }

            return RedirectToAction("Index", "Home");
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

            var result = await _authenticationService.SignInAsync(model.Username, model.Password, false, SignInAsync);
            if (!result.IsSuccess)
            {
                var errorMessage = GetErrorMessage(result.ErrorType);
                ModelState.AddModelError("", errorMessage);

                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> SignOut()
        {
            await _authenticationService.SignOutAsync(SignOutAsync);

            return RedirectToAction("Index", "Home");
        }


        private async Task SignInAsync(string username, int userId, bool rememberMe)
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

        private async Task SignOutAsync()
        {
            await HttpContext.SignOutAsync();
        }

        private static SignUpDto CreateDto(SignUpViewModel model)
        {
            var signUpDto = new SignUpDto(
                model.Username,
                model.Password,
                model.PasswordConfirmation,
                model.Email);

            return signUpDto;
        }

        private static string GetErrorMessage(SignUpErrorType error)
        {
            switch (error)
            {
                case SignUpErrorType.InvalidUsername:
                    return "Invalid username";
                case SignUpErrorType.InvalidPassword:
                    return "Invalid password";
                case SignUpErrorType.InvalidEmail:
                    return "Invalid email";
                case SignUpErrorType.DuplicateUsername:
                    return "Duplicate username";
                case SignUpErrorType.PasswordAndConfirmationNotSame:
                    return "Password and it's confirmation are different";
                default:
                    throw new ArgumentOutOfRangeException(nameof(error), error, null);
            }
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