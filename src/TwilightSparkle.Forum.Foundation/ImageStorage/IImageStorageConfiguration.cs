using System.Collections.Generic;

namespace TwilightSparkle.Forum.Foundation.ImageStorage
{
    public interface IImageStorageConfiguration
    {
        string ImagesDirectory { get; }

        int MaximumImageSize { get; }

        IReadOnlyCollection<string> AllowedImageMediaTypes { get; }
    }
}