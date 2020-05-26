namespace TwilightSparkle.Forum.DataContracts.Images
{
    public static class SaveImageApiErrorCodes
    {
        public const string EmptyFilePath = "incorrect_input_file_path";
        public const string TooBigFile = "too_big_image";
        public const string NotAllowedMediaType = "incorrect_image_type";
        public const string InvalidImage = "invalid_image";
    }
}