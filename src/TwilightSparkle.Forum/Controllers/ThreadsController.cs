using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TwilightSparkle.Forum.DomainModel.Entities;
using TwilightSparkle.Forum.Foundation.ThreadsManagement;
using TwilightSparkle.Forum.Models.Common;
using TwilightSparkle.Forum.Models.Threads;

namespace TwilightSparkle.Forum.Controllers
{
    [Authorize]
    public class ThreadsController : Controller
    {
        private readonly IThreadsManagementService _threadsManagementService;


        public ThreadsController(IThreadsManagementService threadsManagementService)
        {
            _threadsManagementService = threadsManagementService;
        }


        [AllowAnonymous]
        public async Task<IActionResult> SectionThreads(string sectionName)
        {
            var sections = await _threadsManagementService.GetSectionsAsync();
            var threads = await _threadsManagementService.GetSectionThreadsAsync(sectionName);
            var model = GetModel(sectionName, sections, threads);

            return View(model);
        }

        public IActionResult CreateThread(string sectionName)
        {
            var model = new CreateThreadViewModel
            {
                SectionName = sectionName
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateThread(CreateThreadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var createResult = await _threadsManagementService.CreateThread(model.Title, model.Content, model.SectionName, User.Identity.Name);
            if (!createResult.IsSuccess)
            {
                var errorMessage = GetErrorMessage(createResult.ErrorType);
                ModelState.AddModelError("", errorMessage);

                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public async Task<IActionResult> ThreadDetails(int threadId)
        {
            var sections = await _threadsManagementService.GetSectionsAsync();
            var thread = await _threadsManagementService.GetThreadAsync(threadId);
            var model = GetModel(sections, thread);

            return View(model);
        }


        private ThreadDetailsViewModel GetModel(IReadOnlyCollection<Section> sections, Thread thread)
        {
            var sectionModels = sections.Select(s => new SectionViewModel
            {
                SectionName = s.Name
            }).ToList();
            var threadModel = new ThreadViewModel
            {
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