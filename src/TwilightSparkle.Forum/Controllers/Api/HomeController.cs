using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using TwilightSparkle.Forum.ControllerExtenstions;
using TwilightSparkle.Forum.DomainModel.Entities;
using TwilightSparkle.Forum.Foundation.ThreadsManagement;
using TwilightSparkle.Forum.Foundation.UserProfile;
using TwilightSparkle.Forum.Models.Common;
using TwilightSparkle.Forum.Models.Home;

namespace TwilightSparkle.Forum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IThreadsManagementService _threadsManagementService;


        public HomeController(IUserProfileService userProfileService, IThreadsManagementService threadsManagementService)
        {
            _userProfileService = userProfileService;
            _threadsManagementService = threadsManagementService;
        }


        [Route("App")]
        public async Task<IActionResult> App()
        {
            var content = await this.RenderViewToStringAsync("/Views/Home/App.cshtml");

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = content
            };
        }

        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var sections = await _threadsManagementService.GetSectionsAsync();
            var threads = await _threadsManagementService.GetPopularThreadsAsync();
            var model = GetModel(sections, threads);

            var content = await this.RenderViewToStringAsync("/Views/Home/Index.cshtml", model);

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = content
            };
        }

        [Route("Profile")]
        public async Task<IActionResult> Profile()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var user = await _userProfileService.GetCurrentUserAsync(CurrentUserIdentityProvider);
            var profileImageExternalId = await _userProfileService.GetCurrentUserImageExternalIdAsync(CurrentUserIdentityProvider);
            var userThreads = await _threadsManagementService.GetUserThreadsAsync(User.Identity.Name);
            var model = GetModel(user, profileImageExternalId, userThreads);

            var content = await this.RenderViewToStringAsync("/Views/Home/Profile.cshtml", model);

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = content
            };
        }

        [HttpPost]
        [Route("SaveChanges")]
        public async Task<IActionResult> SaveChanges([FromForm]UserProfileViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var userProfileDto = new UpdateUserProfileDto(model.ImageExternalId);

            var updateResult = await _userProfileService.UpdateUserProfileAsync(CurrentUserIdentityProvider, userProfileDto);
            if (!updateResult.IsSuccess)
            {
                var errorMessage = GetErrorMessage(updateResult.ErrorType);

                return new ContentResult
                {
                    ContentType = "application/json",
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Content = errorMessage
                };
            }

            return Ok();
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
                ThreadId = t.Id,
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
            return error switch
            {
                UpdateProfileErrorType.InvalidProfileImage => "Invalid profile image",
                _ => throw new ArgumentOutOfRangeException(nameof(error), error, null),
            };
        }
    }
}