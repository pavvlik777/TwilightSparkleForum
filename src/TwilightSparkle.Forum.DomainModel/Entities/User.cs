using System.Collections.Generic;

namespace TwilightSparkle.Forum.DomainModel.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string Email { get; set; }

        public int? ProfileImageId { get; set; }

        public UploadedImage ProfileImage { get; set; }

        public ICollection<Thread> Threads { get; set; }

        public ICollection<LikeDislike> Likes { get; set; }

        public ICollection<Commentary> Commentaries { get; set; }
    }
}