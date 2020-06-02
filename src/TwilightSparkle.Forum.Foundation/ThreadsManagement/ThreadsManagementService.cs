using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using TwilightSparkle.Common.Services;
using TwilightSparkle.Forum.DomainModel.Entities;
using TwilightSparkle.Forum.Repository.Interfaces;

namespace TwilightSparkle.Forum.Foundation.ThreadsManagement
{
    public class ThreadsManagementService : IThreadsManagementService
    {
        private readonly IForumUnitOfWork _unitOfWork;


        public ThreadsManagementService(IForumUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<IReadOnlyCollection<Section>> GetSectionsAsync()
        {
            var sectionsRepository = _unitOfWork.GetRepository<Section>();
            var sections = await sectionsRepository.GetWhereAsync(s => !string.IsNullOrEmpty(s.Name));

            return sections;
        }

        public async Task<IReadOnlyCollection<Thread>> GetPopularThreadsAsync()
        {
            var threadsRepository = _unitOfWork.GetRepository<Thread>();
            var threads = await threadsRepository.GetWhereAsync(t => !string.IsNullOrEmpty(t.Title), t => t.Author, t => t.Section);

            return threads;
        }

        public async Task<IReadOnlyCollection<Thread>> GetSectionThreadsAsync(string sectionName)
        {
            var threadsRepository = _unitOfWork.GetRepository<Thread>();
            var threads = await threadsRepository.GetWhereAsync(t => t.Section.Name == sectionName, t => t.Author, t => t.Section);

            return threads;
        }

        public async Task<IReadOnlyCollection<Thread>> GetUserThreadsAsync(string username)
        {
            var threadsRepository = _unitOfWork.GetRepository<Thread>();
            var threads = await threadsRepository.GetWhereAsync(t => t.Author.Username == username, t => t.Author, t => t.Section);

            return threads;
        }

        public async Task<Thread> GetThreadAsync(int threadId)
        {
            var threadsRepository = _unitOfWork.GetRepository<Thread>();
            var thread = await threadsRepository.GetFirstOrDefaultAsync(t => t.Id == threadId, t => t.Author, t => t.Section);

            return thread;
        }

        public async Task<ServiceResult<CreateThreadErrorType>> CreateThread(string title, string content, string sectionName, string authorNickname)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return ServiceResult.CreateFailed(CreateThreadErrorType.InvalidTitle);
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                return ServiceResult.CreateFailed(CreateThreadErrorType.InvalidContent);
            }

            var threadsRepository = _unitOfWork.GetRepository<Thread>();
            var sectionsRepository = _unitOfWork.GetRepository<Section>();
            var usersRepository = _unitOfWork.UserRepository;

            var author = await usersRepository.GetFirstOrDefaultAsync(u => u.Username == authorNickname);
            var section = await sectionsRepository.GetFirstOrDefaultAsync(s => s.Name == sectionName);

            var thread = new Thread
            {
                Title = title,
                Content = content,
                Author = author,
                Section = section
            };

            threadsRepository.Create(thread);

            await _unitOfWork.SaveAsync();

            return ServiceResult<CreateThreadErrorType>.CreateSuccess();
        }
    }
}
