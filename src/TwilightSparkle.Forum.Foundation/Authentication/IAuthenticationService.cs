using System.Threading.Tasks;

using TwilightSparkle.Common.Services;

namespace TwilightSparkle.Forum.Foundation.Authentication
{
    public interface IAuthenticationService
    {
        Task<ServiceResult<SignUpErrorType>> SignUpAsync(SignUpDto signUpDto);

        Task<ServiceResult<SignInErrorType>> SignInAsync(string username, string password, bool rememberMe, SignInHandler signInHandler);

        Task SignOutAsync(SignOutHandler signOutHandler);
    }
}