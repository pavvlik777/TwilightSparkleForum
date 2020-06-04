namespace TwilightSparkle.Forum.Foundation.ImageService
{
    public class GetImageSizeResult
    {
        public bool IsSuccessful { get; }

        public GetImageSizeError Error { get; }

        public int Width { get; }

        public int Height { get; }


        private GetImageSizeResult(bool isSuccessful, GetImageSizeError error, int width, int height)
        {
            IsSuccessful = isSuccessful;
            Error = error;
            Width = width;
            Height = height;
        }


        public static GetImageSizeResult CreateSuccessful(int width, int height)
        {
            return new GetImageSizeResult(true, default, width, height);
        }

        public static GetImageSizeResult CreateUnsuccessful(GetImageSizeError error)
        {
            return new GetImageSizeResult(false, error, default, default);
        }
    }
}