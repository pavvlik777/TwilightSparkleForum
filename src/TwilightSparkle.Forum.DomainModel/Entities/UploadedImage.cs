using System;

namespace TwilightSparkle.Forum.DomainModel.Entities
{
    public class UploadedImage
    {
        public int Id { get; set; }

        public string ExternalId { get; set; }

        public string FilePath { get; set; }

        public string MediaType { get; set; }
    }
}