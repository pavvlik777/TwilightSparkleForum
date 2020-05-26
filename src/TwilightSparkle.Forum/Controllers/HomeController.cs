using System;
using System.Security.Principal;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TwilightSparkle.Forum.DomainModel.Entities;
using TwilightSparkle.Forum.Foundation.UserProfile;
using TwilightSparkle.Forum.Models.Home;

namespace TwilightSparkle.Forum.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserProfileService _userProfileService;


        public HomeController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }


        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Profile()
        {
            var user = await _userProfileService.GetCurrentUserAsync(CurrentUserIdentityProvider);
            var profileImageExternalId = await _userProfileService.GetCurrentUserImageExternalIdAsync(CurrentUserIdentityProvider);
            var model = GetModel(user, profileImageExternalId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveChanges(UserProfileViewModel model)
        {
            var userProfileDto = new UpdateUserProfileDto(model.ImageExternalId);

            var updateResult = await _userProfileService.UpdateUserProfileAsync(CurrentUserIdentityProvider, userProfileDto);
            if (!updateResult.IsSuccess)
            {
                var errorMessage = GetErrorMessage(updateResult.ErrorType);
                ModelState.AddModelError("", errorMessage);

                return View("Profile", model);
            }

            return RedirectToAction("Profile");
        }


        private IIdentity CurrentUserIdentityProvider()
        {
            return User.Identity;
        }

        private UserProfileViewModel GetModel(User user, string profileImageExternalId)
        {
            var imageUrl = Url.Action("Get", "Images", new { id = profileImageExternalId }, Request.Scheme);
            var uploadImageUrl = Url.Action("UploadImage", "Images", null, Request.Scheme);
            var model = new UserProfileViewModel
            {
                Username = user.Username,
                Email = user.Email,
                ProfileImageUrl = imageUrl,
                UploadImageUrl = uploadImageUrl,
                ImageExternalId = profileImageExternalId
            };

            return model;
        }

        private static string GetErrorMessage(UpdateProfileErrorType error)
        {
            switch (error)
            {
                case UpdateProfileErrorType.InvalidProfileImage:
                    return "Invalid profile image";
                default:
                    throw new ArgumentOutOfRangeException(nameof(error), error, null);
            }
        }
    }
}