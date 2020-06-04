using System;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

using TwilightSparkle.Forum.DomainModel.Entities;
using TwilightSparkle.Forum.Repository.Interfaces;

namespace TwilightSparkle.Forum.Foundation.ImageStorage
{
    public class ImageStorageService : IImageStorageService
    {
        private readonly IForumUnitOfWork _unitOfWork;
        private readonly IImageStorageConfiguration _configuration;


        public ImageStorageService(IForumUnitOfWork unitOfWork, IImageStorageConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }


        public async Task<SaveImageResult> SaveImageAsync(string filePath, Stream imageStream)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return SaveImageResult.CreateUnsuccessful(SaveImageError.EmptyFilePath);
            }
            if (imageStream.Length > _configuration.MaximumImageSize)
            {
                return SaveImageResult.CreateUnsuccessful(SaveImageError.TooBigImage);
            }
            var imageExtension = Path.GetExtension(filePath);
            var mediaType = MimeMapping.MimeUtility.GetMimeMapping(filePath);
            var isMediaTypeAllowed = _configuration.AllowedImageMediaTypes.Any(t => t == mediaType);
            if (!isMediaTypeAllowed)
            {
                return SaveImageResult.CreateUnsuccessful(SaveImageError.NotAllowedMediaType);
            }
            var externalId = Guid.NewGuid().ToString();
            var uploadedImageName = $"{externalId}{imageExtension}";

            var uploadedImage = new UploadedImage
            {
                FilePath = uploadedImageName,
                ExternalId = externalId,
                MediaType = mediaType
            };

            Directory.CreateDirectory(_configuration.ImagesDirectory);
            var uploadPath = Path.Combine(_configuration.ImagesDirectory, uploadedImageName);
            using (var outputStream = new FileStream(uploadPath, FileMode.Create))
            {
                await imageStream.CopyToAsync(outputStream);
            }

            var imagesRepository = _unitOfWork.GetRepository<UploadedImage>();
            imagesRepository.Create(uploadedImage);
            await _unitOfWork.SaveAsync();

            return SaveImageResult.CreateSuccessful(uploadedImage.ExternalId);
        }

        public async Task<LoadImageResult> LoadImageAsync(string externalId)
        {
            var imagesRepository = _unitOfWork.GetRepository<UploadedImage>();
            var imageDetails = await imagesRepository.GetSingleOrDefaultAsync(i => i.ExternalId == externalId);
            if (imageDetails == null)
            {
                return LoadImageResult.CreateUnsuccessful(LoadImageError.IncorrectExternalId);
            }

            var relativePath = Path.Combine(_configuration.ImagesDirectory, imageDetails.FilePath);
            var fullPath = Path.GetFullPath(relativePath);
            var isImageExists = File.Exists(fullPath);

            return isImageExists
                ? LoadImageResult.CreateSuccessful(fullPath, imageDetails.MediaType)
                : LoadImageResult.CreateUnsuccessful(LoadImageError.ImageNotExists);
        }

        public async Task<LoadImageResult> LoadImageForCurrentUserAsync(IIdentity identity)
        {
            var userRepository = _unitOfWork.UserRepository;
            var currentUser = await userRepository.GetSingleOrDefaultAsync(u => u.Username == identity.Name, u => u.ProfileImage);

            var imageExternalId = "default-profile-image";
            if (currentUser.ProfileImageId.HasValue)
            {
                imageExternalId = currentUser.ProfileImage.ExternalId;
            }

            var imagesRepository = _unitOfWork.GetRepository<UploadedImage>();
            var imageDetails = await imagesRepository.GetSingleOrDefaultAsync(i => i.ExternalId == imageExternalId);
            if (imageDetails == null)
            {
                return LoadImageResult.CreateUnsuccessful(LoadImageError.IncorrectExternalId);
            }

            var relativePath = Path.Combine(_configuration.ImagesDirectory, imageDetails.FilePath);
            var fullPath = Path.GetFullPath(relativePath);
            var isImageExists = File.Exists(fullPath);

            return isImageExists
                ? LoadImageResult.CreateSuccessful(fullPath, imageDetails.MediaType)
                : LoadImageResult.CreateUnsuccessful(LoadImageError.ImageNotExists);
        }
    }
}