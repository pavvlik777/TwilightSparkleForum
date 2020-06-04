using System.IO;

namespace TwilightSparkle.Forum.Foundation.ImageService
{
    public interface IImageService
    {
        GetImageSizeResult GetImageSize(Stream imageStream);
    }
}