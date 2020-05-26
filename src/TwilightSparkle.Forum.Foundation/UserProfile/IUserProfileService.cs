using System.Threading.Tasks;

using TwilightSparkle.Common.Services;
using TwilightSparkle.Forum.DomainModel.Entities;

namespace TwilightSparkle.Forum.Foundation.UserProfile
{
    public interface IUserProfileService
    {
        Task<User> GetCurrentUserAsync(CurrentUserIdentityProvider currentUserIdentityProvider);

        Task<string> GetCurrentUserImageExternalIdAsync(CurrentUserIdentityProvider currentUserIdentityProvider);

        Task<ServiceResult<UpdateProfileErrorType>> UpdateUserProfileAsync(CurrentUserIdentityProvider currentUserIdentityProvider, UpdateUserProfileDto dto);
    }
}