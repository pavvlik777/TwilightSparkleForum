using System.Collections.Generic;

using TwilightSparkle.Forum.Models.Common;

namespace TwilightSparkle.Forum.Models.Threads
{
    public class SectionThreadsViewModel
    {
        public string SectionName { get; set; }

        public IReadOnlyCollection<SectionViewModel> Sections { get; set; }

        public IReadOnlyCollection<ThreadViewModel> SectionThreads { get; set; }
    }
}
