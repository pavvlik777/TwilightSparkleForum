using System;

namespace TwilightSparkle.Forum.Models.Threads
{
    public class CommentThreadViewModel
    {
        public string Content { get; set; }

        public DateTime CommentTime { get; set; }

        public string AuthorNickname { get; set; }
    }
}
