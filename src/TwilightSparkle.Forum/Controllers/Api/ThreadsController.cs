using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using TwilightSparkle.Forum.ControllerExtenstions;
using TwilightSparkle.Forum.DomainModel.Entities;
using TwilightSparkle.Forum.Foundation.ThreadsManagement;
using TwilightSparkle.Forum.Models.Common;
using TwilightSparkle.Forum.Models.Threads;

namespace TwilightSparkle.Forum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThreadsController : Controller
    {
        private readonly IThreadsManagementService _threadsManagementService;


        public ThreadsController(IThreadsManagementService threadsManagementService)
        {
            _threadsManagementService = threadsManagementService;
        }


        [Route("SectionThreads")]
        public async Task<IActionResult> SectionThreads([FromQuery]string sectionName)
        {
            var sections = await _threadsManagementService.GetSectionsAsync();
            var threads = await _threadsManagementService.GetSectionThreadsAsync(sectionName);
            var model = GetModel(sectionName, sections, threads);

            var content = await this.RenderViewToStringAsync("/Views/Threads/SectionThreads.cshtml", model);

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = content
            };
        }

        [Route("CreateThread")]
        public async Task<IActionResult> CreateThread([FromQuery]string sectionName)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var model = new CreateThreadViewModel
            {
                SectionName = sectionName
            };

            var content = await this.RenderViewToStringAsync("/Views/Threads/CreateThread.cshtml", model);

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = content
            };
        }

        [HttpPost]
        [Route("CreateThread")]
        public async Task<IActionResult> CreateThread([FromForm]CreateThreadViewModel model, [FromForm]string unparsedContent)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            if (string.IsNullOrWhiteSpace(unparsedContent))
            {
                return new ContentResult
                {
                    ContentType = "text/html",
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Content = "Content can't be empty"
                };
            }

            var createResult = await _threadsManagementService.CreateThreadAsync(model.Title, model.Content, model.SectionName, User.Identity.Name);
            if (!createResult.IsSuccess)
            {
                var errorMessage = GetErrorMessage(createResult.ErrorType);

                return new ContentResult
                {
                    ContentType = "text/html",
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Content = errorMessage
                };
            }

            return Ok();
        }

        [Route("ThreadsDetails")]
        public async Task<IActionResult> ThreadDetails(int threadId)
        {
            var model = await GetThreadViewModelAsync(threadId);

            var content = await this.RenderViewToStringAsync("/Views/Threads/ThreadDetails.cshtml", model);

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = content
            };
        }

        [HttpPost]
        [Route("DeleteThread")]
        public async Task<IActionResult> DeleteThread([FromQuery]int threadId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var deleteResult = await _threadsManagementService.DeleteThreadAsync(threadId, User.Identity.Name);
            if (!deleteResult.IsSuccess)
            {
                var errorMessage = GetErrorMessage(deleteResult.ErrorType);

                return new ContentResult
                {
                    ContentType = "text/html",
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Content = errorMessage
                };
            }

            return Ok();
        }

        [HttpPost]
        [Route("LikeThread")]
        public async Task<IActionResult> LikeThread([FromQuery]int threadId, bool isLike)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            await _threadsManagementService.LikeOrDislikeThreadAsync(threadId, isLike, User.Identity.Name);

            var model = await GetThreadViewModelAsync(threadId);

            var content = await this.RenderViewToStringAsync("/Views/Threads/ThreadLikesSection.cshtml", model);

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = content
            };
        }

        [HttpPost]
        [Route("CommentThread")]
        public async Task<IActionResult> CommentThread([FromForm]int threadId, [FromForm]string content, [FromForm]string unparsedContent)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            if (string.IsNullOrWhiteSpace(unparsedContent))
            {
                return new ContentResult
                {
                    ContentType = "text/html",
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Content = "Content can't be empty"
                };
            }

            var commentResult = await _threadsManagementService.CommentThreadAsync(threadId, content, User.Identity.Name);
            if (!commentResult.IsSuccess)
            {
                var errorMessage = GetErrorMessage(commentResult.ErrorType);

                return new ContentResult
                {
                    ContentType = "text/html",
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Content = errorMessage
                };
            }

            var model = await GetThreadViewModelAsync(threadId);
            var commentSection = await this.RenderViewToStringAsync("/Views/Threads/ThreadCommentSection.cshtml", model);

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = commentSection
            };
        }


        private async Task<ThreadDetailsViewModel> GetThreadViewModelAsync(int threadId)
        {
            var sections = await _threadsManagementService.GetSectionsAsync();
            var thread = await _threadsManagementService.GetThreadAsync(threadId);
            var comments = await _threadsManagementService.GetCommentariesAsync(threadId);
            var amountOfLikes = await _threadsManagementService.GetAmountOfLikesAsync(threadId);
            var likeStatus = 0;
            if (User.Identity.IsAuthenticated)
            {
                var like = await _threadsManagementService.GetLikeAsync(threadId, User.Identity.Name);
                likeStatus = like == null ? 0 : like.IsLike ? 1 : -1;
            }
            var model = GetModel(sections, comments, thread, amountOfLikes, likeStatus);

            return model;
        }

        private ThreadDetailsViewModel GetModel(IReadOnlyCollection<Section> sections, IReadOnlyCollection<Commentary> comments, Thread thread, int likesAmount, int likeStatus)
        {
            var sectionModels = sections.Select(s => new SectionViewModel
            {
                SectionName = s.Name
            }).ToList();
            var commentsModels = comments.OrderByDescending(c => c.CommentTime).Select(c => new CommentThreadViewModel
            {
                AuthorNickname = c.Author.Username,
                CommentTime = c.CommentTime,
                Content = c.Content
            }).ToList();
            var threadModel = new ThreadViewModel
            {
                ThreadId = thread.Id,
                AuthorNickname = thread.Author.Username,
                Content = thread.Content,
                Title = thread.Title
            };

            var isAuthor = User.Identity.IsAuthenticated ? thread.Author.Username == User.Identity.Name : false;
            var model = new ThreadDetailsViewModel
            {
                LikesAmount = likesAmount,
                LikeStatus = likeStatus,
                IsAuthor = isAuthor,
                Sections = sectionModels,
                Comments = commentsModels,
                Thread = threadModel
            };

            return model;
        }

        private SectionThreadsViewModel GetModel(string sectionName, IReadOnlyCollection<Section> sections, IReadOnlyCollection<Thread> sectionThreads)
        {
            var sectionModels = sections.Select(s => new SectionViewModel
            {
                SectionName = s.Name
            }).ToList();
            var threadModels = sectionThreads.Select(t => new ThreadViewModel
            {
                ThreadId = t.Id,
                Title = t.Title,
                Content = t.Content,
                AuthorNickname = t.Author.Username
            }).ToList();
            var model = new SectionThreadsViewModel
            {
                SectionName = sectionName,
                Sections = sectionModels,
                SectionThreads = threadModels
            };

            return model;
        }

        private string GetErrorMessage(CommentThreadError error)
        {
            switch (error)
            {
                case CommentThreadError.InvalidContent:
                    return "Invalid content";
                default:
                    throw new ArgumentOutOfRangeException(nameof(error), error, null);
            }
        }

        private string GetErrorMessage(CreateThreadErrorType error)
        {
            switch (error)
            {
                case CreateThreadErrorType.InvalidTitle:
                    return "Invalid title";
                case CreateThreadErrorType.InvalidContent:
                    return "Invalid content";
                default:
                    throw new ArgumentOutOfRangeException(nameof(error), error, null);
            }
        }

        private string GetErrorMessage(DeleteThreadErrorType error)
        {
            switch (error)
            {
                case DeleteThreadErrorType.InvalidThread:
                    return "Invalid thread";
                case DeleteThreadErrorType.NotAuthor:
                    return "Forbidden";
                default:
                    throw new ArgumentOutOfRangeException(nameof(error), error, null);
            }
        }
    }
}