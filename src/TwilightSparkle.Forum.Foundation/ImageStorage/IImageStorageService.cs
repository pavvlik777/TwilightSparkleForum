using System.IO;
using System.Threading.Tasks;

namespace TwilightSparkle.Forum.Foundation.ImageStorage
{
    public interface IImageStorageService
    {
        Task<SaveImageResult> SaveImageAsync(string filePath, Stream imageStream);

        Task<LoadImageResult> LoadImageAsync(string externalId);
    }
}