using System.Threading.Tasks;

using TwilightSparkle.Common.Services;
using TwilightSparkle.Forum.DomainModel.Entities;
using TwilightSparkle.Forum.Repository.Interfaces;

namespace TwilightSparkle.Forum.Foundation.UserProfile
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IForumUnitOfWork _unitOfWork;


        public UserProfileService(IForumUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<User> GetCurrentUserAsync(CurrentUserIdentityProvider currentUserIdentityProvider)
        {
            var userRepository = _unitOfWork.UserRepository;
            var currentUserIdentity = currentUserIdentityProvider();
            var currentUser = await userRepository.GetSingleOrDefaultAsync(u => u.Username == currentUserIdentity.Name);

            return currentUser;
        }

        public async Task<string> GetCurrentUserImageExternalIdAsync(CurrentUserIdentityProvider currentUserIdentityProvider)
        {
            var userRepository = _unitOfWork.UserRepository;
            var currentUserIdentity = currentUserIdentityProvider();
            var currentUser = await userRepository.GetSingleOrDefaultAsync(u => u.Username == currentUserIdentity.Name, u => u.ProfileImage);
            if (currentUser.ProfileImageId.HasValue)
            {
                return currentUser.ProfileImage.ExternalId;
            }

            return "default-profile-image";
        }

        public async Task<ServiceResult<UpdateProfileErrorType>> UpdateUserProfileAsync(CurrentUserIdentityProvider currentUserIdentityProvider, UpdateUserProfileDto dto)
        {
            var userRepository = _unitOfWork.UserRepository;
            var imageRepostiory = _unitOfWork.GetRepository<UploadedImage>();

            var newImage = await imageRepostiory.GetFirstOrDefaultAsync(i => i.ExternalId == dto.UserProfileImageExternalId);
            if (newImage == null)
            {
                return ServiceResult.CreateFailed(UpdateProfileErrorType.InvalidProfileImage);
            }

            var currentUser = await GetCurrentUserAsync(currentUserIdentityProvider);
            currentUser.ProfileImageId = newImage.Id;
            userRepository.Update(currentUser);
            await _unitOfWork.SaveAsync();

            return ServiceResult.CreateSuccess<UpdateProfileErrorType>();
        }
    }
}
