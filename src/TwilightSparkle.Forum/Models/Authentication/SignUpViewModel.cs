using System.ComponentModel.DataAnnotations;

namespace TwilightSparkle.Forum.Models.Authentication
{
    public class SignUpViewModel
    {
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Password confirmation")]
        public string PasswordConfirmation { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}