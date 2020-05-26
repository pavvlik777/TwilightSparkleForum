using System.ComponentModel.DataAnnotations;

namespace TwilightSparkle.Forum.Models.Home
{
    public class UserProfileViewModel
    {
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        public string ProfileImageUrl { get; set; }

        public string ImageExternalId { get; set; }

        public string UploadImageUrl { get; set; }
    }
}
