using System.Collections.Generic;

using TwilightSparkle.Forum.Models.Common;

namespace TwilightSparkle.Forum.Models.Home
{
    public class IndexViewModel
    {
        public IReadOnlyCollection<SectionViewModel> Sections { get; set; }

        public IReadOnlyCollection<ThreadViewModel> PopularThreads { get; set; }
    }
}
