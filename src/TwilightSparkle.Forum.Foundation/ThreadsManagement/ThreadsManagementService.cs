using System;
using System.Collections.Generic;
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

        public async Task<ServiceResult<CreateThreadErrorType>> CreateThreadAsync(string title, string content, string sectionName, string authorNickname)
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

        public async Task<ServiceResult<DeleteThreadErrorType>> DeleteThreadAsync(int threadId, string username)
        {
            var threadsRepository = _unitOfWork.GetRepository<Thread>();
            var thread = await threadsRepository.GetFirstOrDefaultAsync(t => t.Id == threadId, t => t.Author, t => t.Section);

            if (thread == null)
            {
                return ServiceResult.CreateFailed(DeleteThreadErrorType.InvalidThread);
            }
            if (thread.Author.Username != username)
            {
                return ServiceResult.CreateFailed(DeleteThreadErrorType.NotAuthor);
            }

            threadsRepository.Delete(thread);

            await _unitOfWork.SaveAsync();

            return ServiceResult<DeleteThreadErrorType>.CreateSuccess();
        }

        public async Task LikeOrDislikeThreadAsync(int threadId, bool isLike, string username)
        {
            var threadsRepository = _unitOfWork.GetRepository<Thread>();
            var thread = await threadsRepository.GetFirstOrDefaultAsync(t => t.Id == threadId, t => t.Author, t => t.Section);
            var usersRepository = _unitOfWork.UserRepository;
            var author = await usersRepository.GetFirstOrDefaultAsync(u => u.Username == username);

            var likesRepository = _unitOfWork.GetRepository<LikeDislike>();
            var currentLike = await likesRepository.GetFirstOrDefaultAsync(l => l.ThreadId == threadId && l.User == author, l => l.Thread, l => l.User);
            if (currentLike == null)
            {
                currentLike = new LikeDislike
                {
                    User = author,
                    Thread = thread,
                    IsLike = isLike
                };

                likesRepository.Create(currentLike);
            }
            else if(currentLike.IsLike != isLike)
            {
                currentLike.IsLike = isLike;

                likesRepository.Update(currentLike);
            }
            else
            {
                likesRepository.Delete(currentLike);
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task<ServiceResult<CommentThreadError>> CommentThreadAsync(int threadId, string content, string authorNickname)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return ServiceResult.CreateFailed(CommentThreadError.InvalidContent);
            }

            var threadsRepository = _unitOfWork.GetRepository<Thread>();
            var thread = await threadsRepository.GetFirstOrDefaultAsync(t => t.Id == threadId, t => t.Author, t => t.Section);
            var usersRepository = _unitOfWork.UserRepository;
            var author = await usersRepository.GetFirstOrDefaultAsync(u => u.Username == authorNickname);

            var commentaryRepository = _unitOfWork.GetRepository<Commentary>();
            var newComment = new Commentary
            {
                Author = author,
                Thread = thread,
                CommentTime = DateTime.UtcNow,
                Content = content
            };
            commentaryRepository.Create(newComment);

            await _unitOfWork.SaveAsync();

            return ServiceResult<CommentThreadError>.CreateSuccess();
        }

        public async Task<int> GetAmountOfLikesAsync(int threadId)
        {
            var likesRepository = _unitOfWork.GetRepository<LikeDislike>();
            var likes = await likesRepository.GetWhereAsync(l => l.ThreadId == threadId && l.IsLike);
            var dislikes = await likesRepository.GetWhereAsync(l => l.ThreadId == threadId && !l.IsLike);

            return likes.Count - dislikes.Count;
        }

        public async Task<LikeDislike> GetLikeAsync(int threadId, string authorNickname)
        {
            var likesRepository = _unitOfWork.GetRepository<LikeDislike>();
            var like = await likesRepository.GetFirstOrDefaultAsync(l => l.ThreadId == threadId && l.User.Username == authorNickname, l => l.Thread, l => l.User);

            return like;
        }

        public async Task<IReadOnlyCollection<Commentary>> GetCommentariesAsync(int threadId)
        {
            var commentaryRepository = _unitOfWork.GetRepository<Commentary>();
            var comments = await commentaryRepository.GetWhereAsync(c => c.ThreadId == threadId, c => c.Thread, c => c.Author);

            return comments;
        }
    }
}
