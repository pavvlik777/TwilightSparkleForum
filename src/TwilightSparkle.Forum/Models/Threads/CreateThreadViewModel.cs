using System.ComponentModel.DataAnnotations;

namespace TwilightSparkle.Forum.Models.Threads
{
    public class CreateThreadViewModel
    {
        public string SectionName { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Content")]
        public string Content { get; set; }
    }
}
