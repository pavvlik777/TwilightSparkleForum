using System.Text.RegularExpressions;
using System.Threading.Tasks;

using TwilightSparkle.Common.Hasher;
using TwilightSparkle.Common.Services;
using TwilightSparkle.Forum.DomainModel.Entities;
using TwilightSparkle.Forum.Repository.Interfaces;

namespace TwilightSparkle.Forum.Foundation.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private const string UsernamePattern = @"^(?=.{8,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$";
        private const string PasswordPattern = @"(?=^.{8,}$)(?=.*\d)(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$";
        private const string EmailPattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        private static readonly Regex UsernameRegex;
        private static readonly Regex PasswordRegex;
        private static readonly Regex EmailRegex;


        private readonly IHasher _hasher;

        private readonly IForumUnitOfWork _unitOfWork;


        public AuthenticationService(IHasher hasher, IForumUnitOfWork unitOfWork)
        {
            _hasher = hasher;

            _unitOfWork = unitOfWork;
        }

        static AuthenticationService()
        {
            UsernameRegex = new Regex(UsernamePattern);
            PasswordRegex = new Regex(PasswordPattern);
            EmailRegex = new Regex(EmailPattern);
        }


        public async Task<ServiceResult<SignUpErrorType>> SignUpAsync(SignUpDto signUpDto)
        {
            var isValidUsername = CheckIfValidUsername(signUpDto.Username);
            if (!isValidUsername)
            {
                return ServiceResult.CreateFailed(SignUpErrorType.InvalidUsername);
            }
            var isValidPassword = CheckIfValidPassword(signUpDto.Password);
            if (!isValidPassword)
            {
                return ServiceResult.CreateFailed(SignUpErrorType.InvalidPassword);
            }
            var isValidEmail = CheckIfValidEmail(signUpDto.Email);
            if (!isValidEmail)
            {
                return ServiceResult.CreateFailed(SignUpErrorType.InvalidEmail);
            }
            var isValidPasswordConfirmation = signUpDto.PasswordConfirmation == signUpDto.Password;
            if (!isValidPasswordConfirmation)
            {
                return ServiceResult.CreateFailed(SignUpErrorType.PasswordAndConfirmationNotSame);
            }

            var userRepository = _unitOfWork.UserRepository;
            var duplicateUser = await userRepository.GetFirstOrDefaultAsync(u => u.Username == signUpDto.Username);
            if (duplicateUser != null)
            {
                return ServiceResult.CreateFailed(SignUpErrorType.DuplicateUsername);
            }
            duplicateUser = await userRepository.GetFirstOrDefaultAsync(u => u.Email == signUpDto.Email);
            if (duplicateUser != null)
            {
                return ServiceResult.CreateFailed(SignUpErrorType.DuplicateEmail);
            }

            var passwordHash = _hasher.GetHash(signUpDto.Password);
            var newUser = new User
            {
                Username = signUpDto.Username,
                Email = signUpDto.Email,
                PasswordHash = passwordHash,
                ProfileImageId = null
            };
            userRepository.Create(newUser);
            await _unitOfWork.SaveAsync();

            return ServiceResult<SignUpErrorType>.CreateSuccess();
        }

        public async Task<ServiceResult<SignInErrorType>> SignInAsync(string username, string password, bool rememberMe, SignInHandler signInHandler)
        {
            var userRepository = _unitOfWork.UserRepository;
            var passwordHash = _hasher.GetHash(password);
            var user = await userRepository.GetFirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == passwordHash);
            if(user == null)
            {
                return ServiceResult.CreateFailed(SignInErrorType.InvalidCredentials);
            }

            await signInHandler(user.Username, user.Id, rememberMe);

            return ServiceResult<SignInErrorType>.CreateSuccess();
        }

        public async Task SignOutAsync(SignOutHandler signOutHandler)
        {
            await signOutHandler();
        }


        private static bool CheckIfValidUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return false;
            }

            var isUsernameValid = UsernameRegex.IsMatch(username);

            return isUsernameValid;
        }

        private static bool CheckIfValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            var isPasswordValid = PasswordRegex.IsMatch(password);

            return isPasswordValid;
        }

        private static bool CheckIfValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            var isEmailValid = EmailRegex.IsMatch(email);

            return isEmailValid;
        }
    }
}