using System;
using System.Drawing;
using System.IO;

namespace TwilightSparkle.Forum.Foundation.ImageService
{
    public class ImageService : IImageService
    {
        public GetImageSizeResult GetImageSize(Stream imageStream)
        {
            try
            {
                using (var image = Image.FromStream(imageStream))
                {
                    imageStream.Position = 0;

                    return GetImageSizeResult.CreateSuccessful(image.Width, image.Height);
                }
            }
            catch (ArgumentException)
            {
                return GetImageSizeResult.CreateUnsuccessful(GetImageSizeError.InvalidImage);
            }
        }
    }
}