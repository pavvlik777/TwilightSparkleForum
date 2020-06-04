using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TwilightSparkle.Forum.DomainModel.Entities;
using TwilightSparkle.Forum.Foundation.ThreadsManagement;
using TwilightSparkle.Forum.Foundation.UserProfile;
using TwilightSparkle.Forum.Models.Common;
using TwilightSparkle.Forum.Models.Home;

namespace TwilightSparkle.Forum.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IThreadsManagementService _threadsManagementService;


        public HomeController(IUserProfileService userProfileService, IThreadsManagementService threadsManagementService)
        {
            _userProfileService = userProfileService;
            _threadsManagementService = threadsManagementService;
        }


        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var sections = await _threadsManagementService.GetSectionsAsync();
            var threads = await _threadsManagementService.GetPopularThreadsAsync();
            var model = GetModel(sections, threads);

            return View(model);
        }

        public async Task<IActionResult> Profile()
        {
            var user = await _userProfileService.GetCurrentUserAsync(CurrentUserIdentityProvider);
            var profileImageExternalId = await _userProfileService.GetCurrentUserImageExternalIdAsync(CurrentUserIdentityProvider);
            var userThreads = await _threadsManagementService.GetUserThreadsAsync(User.Identity.Name);
            var model = GetModel(user, profileImageExternalId, userThreads);

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

                return RedirectToAction("Profile");
            }

            return RedirectToAction("Profile");
        }


        private IIdentity CurrentUserIdentityProvider()
        {
            return User.Identity;
        }

        private IndexViewModel GetModel(IReadOnlyCollection<Section> sections, IReadOnlyCollection<Thread> popularThreads)
        {
            var sectionModels = sections.Select(s => new SectionViewModel
            {
                SectionName = s.Name
            }).ToList();
            var threadModels = popularThreads.Select(t => new ThreadViewModel
            {
                Title = t.Title,
                Content = t.Content,
                AuthorNickname = t.Author.Username
            }).ToList();
            var model = new IndexViewModel
            {
                Sections = sectionModels,
                PopularThreads = threadModels
            };

            return model;
        }

        private UserProfileViewModel GetModel(User user, string profileImageExternalId, IReadOnlyCollection<Thread> userThreads)
        {
            var imageUrl = Url.Action("Get", "Images", new { id = profileImageExternalId }, Request.Scheme);
            var uploadImageUrl = Url.Action("UploadImage", "Images", null, Request.Scheme);
            var threads = userThreads.Select(t => new BaseThreadInfoViewModel
            {
                AuthorNickname = t.Author.Username,
                ThreadId = t.Id,
                Title = t.Title
            }).ToList();

            var model = new UserProfileViewModel
            {
                Username = user.Username,
                Email = user.Email,
                ProfileImageUrl = imageUrl,
                UploadImageUrl = uploadImageUrl,
                ImageExternalId = profileImageExternalId,
                UserThreads = threads
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