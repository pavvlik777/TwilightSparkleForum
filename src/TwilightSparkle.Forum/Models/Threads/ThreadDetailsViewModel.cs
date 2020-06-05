using System.Collections.Generic;

using TwilightSparkle.Forum.Models.Common;

namespace TwilightSparkle.Forum.Models.Threads
{
    public class ThreadDetailsViewModel
    {
        public bool IsAuthor { get; set; }

        public IReadOnlyCollection<SectionViewModel> Sections { get; set; }

        public ThreadViewModel Thread { get; set; }
    }
}
