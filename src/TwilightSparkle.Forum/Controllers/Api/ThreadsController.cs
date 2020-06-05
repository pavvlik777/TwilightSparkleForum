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
        public async Task<IActionResult> CreateThread([FromForm]CreateThreadViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var createResult = await _threadsManagementService.CreateThread(model.Title, model.Content, model.SectionName, User.Identity.Name);
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
            var sections = await _threadsManagementService.GetSectionsAsync();
            var thread = await _threadsManagementService.GetThreadAsync(threadId);
            var model = GetModel(sections, thread);

            var content = await this.RenderViewToStringAsync("/Views/Threads/ThreadDetails.cshtml", model);

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = content
            };
        }


        private ThreadDetailsViewModel GetModel(IReadOnlyCollection<Section> sections, Thread thread)
        {
            var sectionModels = sections.Select(s => new SectionViewModel
            {
                SectionName = s.Name
            }).ToList();
            var threadModel = new ThreadViewModel
            {
                ThreadId = thread.Id,
                AuthorNickname = thread.Author.Username,
                Content = thread.Content,
                Title = thread.Title
            };

            var model = new ThreadDetailsViewModel
            {
                Sections = sectionModels,
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
    }
}