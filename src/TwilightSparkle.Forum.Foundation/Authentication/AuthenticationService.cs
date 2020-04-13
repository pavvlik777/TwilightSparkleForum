using System.Threading.Tasks;

using TwilightSparkle.Common.Hasher;
using TwilightSparkle.Common.Services;
using TwilightSparkle.Forum.Repository.Interfaces;

namespace TwilightSparkle.Forum.Foundation.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHasher _hasher;

        private readonly IForumUnitOfWork _unitOfWork;


        public AuthenticationService(IHasher hasher, IForumUnitOfWork unitOfWork)
        {
            _hasher = hasher;

            _unitOfWork = unitOfWork;
        }


        public async Task<ServiceResult<SignInErrorType>> SignInAsync(string username, string password, bool rememberMe, AuthenticationHandler authenticationHandler)
        {
            var userRepository = _unitOfWork.UserRepository;
            var passwordHash = _hasher.GetHash(password);
            var user = await userRepository.GetFirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == passwordHash);
            if(user == null)
            {
                return ServiceResult.CreateFailed(SignInErrorType.InvalidCredentials);
            }

            await authenticationHandler(user.Username, user.Id, rememberMe);

            return ServiceResult<SignInErrorType>.CreateSuccess();
        }


        private static bool ValidateUsername(string username)
        {
            return true;
        }

        private static bool ValidatePassword(string password)
        {
            return true;
        }
    }
}