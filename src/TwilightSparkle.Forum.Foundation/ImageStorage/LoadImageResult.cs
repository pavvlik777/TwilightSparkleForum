namespace TwilightSparkle.Forum.Foundation.ImageStorage
{
    public class LoadImageResult
    {
        public bool IsSuccessful { get; }

        public LoadImageError Error { get; }

        public string FilePath { get; }

        public string FileMediaType { get; }


        private LoadImageResult(bool isSuccessful, LoadImageError error, string filePath, string fileMediaType)
        {
            IsSuccessful = isSuccessful;
            Error = error;
            FilePath = filePath;
            FileMediaType = fileMediaType;
        }


        public static LoadImageResult CreateSuccessful(string filePath, string fileMediaType)
        {
            return new LoadImageResult(true, default, filePath, fileMediaType);
        }

        public static LoadImageResult CreateUnsuccessful(LoadImageError error)
        {
            return new LoadImageResult(false, error, string.Empty, string.Empty);
        }
    }
}