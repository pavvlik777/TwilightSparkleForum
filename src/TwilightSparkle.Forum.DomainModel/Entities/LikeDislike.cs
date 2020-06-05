namespace TwilightSparkle.Forum.DomainModel.Entities
{
    public class LikeDislike
    {
        public int Id { get; set; }

        public bool IsLike { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int ThreadId { get; set; }

        public Thread Thread { get; set; }
    }
}
