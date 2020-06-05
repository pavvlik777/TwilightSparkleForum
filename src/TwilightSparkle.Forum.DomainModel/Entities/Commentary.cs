using System;

namespace TwilightSparkle.Forum.DomainModel.Entities
{
    public class Commentary
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CommentTime { get; set; }

        public int AuthorId { get; set; }

        public User Author { get; set; }

        public int ThreadId { get; set; }

        public Thread Thread { get; set; }
    }
}
