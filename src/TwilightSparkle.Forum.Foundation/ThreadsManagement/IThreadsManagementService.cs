using System.Collections.Generic;
using System.Threading.Tasks;

using TwilightSparkle.Common.Services;
using TwilightSparkle.Forum.DomainModel.Entities;

namespace TwilightSparkle.Forum.Foundation.ThreadsManagement
{
    public interface IThreadsManagementService
    {
        Task<IReadOnlyCollection<Section>> GetSectionsAsync();

        Task<IReadOnlyCollection<Thread>> GetPopularThreadsAsync();

        Task<IReadOnlyCollection<Thread>> GetSectionThreadsAsync(string sectionName);

        Task<IReadOnlyCollection<Thread>> GetUserThreadsAsync(string username);

        Task<Thread> GetThreadAsync(int threadId);

        Task<ServiceResult<CreateThreadErrorType>> CreateThreadAsync(string title, string content, string sectionName, string authorNickname);

        Task<ServiceResult<DeleteThreadErrorType>> DeleteThreadAsync(int threadId, string username);
    }
}
