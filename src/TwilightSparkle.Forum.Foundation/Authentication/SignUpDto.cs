namespace TwilightSparkle.Forum.Foundation.Authentication
{
    public class SignUpDto
    {
        public string Username { get; }

        public string Password { get; }

        public string PasswordConfirmation { get; }

        public string Email { get; }


        public SignUpDto(
            string username,
            string password,
            string passwordConfirmation,
            string email)
        {
            Username = username;
            Password = password;
            PasswordConfirmation = passwordConfirmation;
            Email = email;
        }
    }
}