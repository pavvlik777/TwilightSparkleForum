using System.Collections.Generic;

namespace TwilightSparkle.Forum.DomainModel.Entities
{
    public class Thread
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int AuthorId { get; set; }

        public User Author { get; set; }

        public int SectionId { get; set; }

        public Section Section { get; set; }

        public ICollection<LikeDislike> Likes { get; set; }

        public ICollection<Commentary> Commentaries { get; set; }
    }
}
