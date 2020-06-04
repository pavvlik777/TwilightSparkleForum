using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwilightSparkle.Forum.ControllerExtenstions;
using TwilightSparkle.Forum.CustomAttributes;
using TwilightSparkle.Forum.Foundation.Authentication;
using TwilightSparkle.Forum.Models.Authentication;

using IAuthenticationService = TwilightSparkle.Forum.Foundation.Authentication.IAuthenticationService;

namespace TwilightSparkle.Forum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;


        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }


        [Route("SignIn")]
        public async Task<IActionResult> SignIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Forbid();
            }

            var model = new SignInViewModel();
            var content = await this.RenderViewToStringAsync("/Views/Authentication/SignIn.cshtml", model);

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = content
            };
        }

        [HttpPost]
        [Route("SignIn")]
        public async Task<IActionResult> SignIn([FromForm] SignInViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Forbid();
            }

            var result = await _authenticationService.SignInAsync(model.Username, model.Password, false, SignInAsync);
            if (!result.IsSuccess)
            {
                var errorMessage = GetErrorMessage(result.ErrorType);

                return new ContentResult
                {
                    ContentType = "application/json",
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    Content = errorMessage
                };
            }

            return Ok();
        }

        [Route("SignUp")]
        public async Task<IActionResult> SignUp()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Forbid();
            }

            var model = new SignUpViewModel();
            var content = await this.RenderViewToStringAsync("/Views/Authentication/SignUp.cshtml", model);

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = content
            };
        }

        [HttpPost]
        [Route("SignUp")]
        public async Task<IActionResult> SignUp([FromForm]SignUpViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Forbid();
            }

            var signUpDto = CreateDto(model);
            var result = await _authenticationService.SignUpAsync(signUpDto);
            if (!result.IsSuccess)
            {
                var errorMessage = GetErrorMessage(result.ErrorType);

                return new ContentResult
                {
                    ContentType = "application/json",
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    Content = errorMessage
                };
            }

            return Ok();
        }

        [HttpPost]
        [Route("SignOut")]
        public async Task<IActionResult> SignOut()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            await _authenticationService.SignOutAsync(SignOutAsync);

            return Ok();
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
                case SignUpErrorType.DuplicateEmail:
                    return "Duplicate email";
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