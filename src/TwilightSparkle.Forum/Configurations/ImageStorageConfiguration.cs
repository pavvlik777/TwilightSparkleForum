using System.Collections.Generic;

using TwilightSparkle.Forum.Foundation.ImageStorage;

namespace TwilightSparkle.Forum.Configurations
{
    public class ImageStorageConfiguration : IImageStorageConfiguration
    {
        public string ImagesDirectory { get; set; }

        public int MaximumImageSize { get; set; }

        public IReadOnlyCollection<string> AllowedImageMediaTypes { get; set; }
    }
}