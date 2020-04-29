using System.ComponentModel.DataAnnotations;

namespace TwilightSparkle.Forum.Models.Authentication
{
    public class SignInViewModel
    {
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}