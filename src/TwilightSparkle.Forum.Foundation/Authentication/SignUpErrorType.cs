namespace TwilightSparkle.Forum.Foundation.Authentication
{
    public enum SignUpErrorType
    {
        InvalidUsername,
        InvalidPassword,
        InvalidEmail,
        DuplicateUsername,
        DuplicateEmail,
        PasswordAndConfirmationNotSame
    }
}