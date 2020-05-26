namespace TwilightSparkle.Forum.Foundation.ImageStorage
{
    public class SaveImageResult
    {
        public bool IsSuccessful { get; }

        public SaveImageError Error { get; }

        public string ExternalId { get; }


        private SaveImageResult(bool isSuccessful, SaveImageError error, string externalId)
        {
            IsSuccessful = isSuccessful;
            Error = error;
            ExternalId = externalId;
        }


        public static SaveImageResult CreateSuccessful(string externalId)
        {
            return new SaveImageResult(true, default, externalId);
        }

        public static SaveImageResult CreateUnsuccessful(SaveImageError error)
        {
            return new SaveImageResult(false, error, string.Empty);
        }
    }
}