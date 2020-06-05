using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using TwilightSparkle.Forum.DataContracts;
using TwilightSparkle.Forum.DataContracts.Images;
using TwilightSparkle.Forum.Foundation.ImageService;
using TwilightSparkle.Forum.Foundation.ImageStorage;

namespace TwilightSparkle.Forum.Controllers.Api
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ImagesController : Controller
    {
        private readonly IImageStorageService _imageStorageService;
        private readonly IImageService _imageService;


        public ImagesController(IImageStorageService imageStorageService, IImageService imageService)
        {
            _imageStorageService = imageStorageService;
            _imageService = imageService;
        }


        [HttpGet("{id}")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Get(string id)
        {
            var loadImageResult = await _imageStorageService.LoadImageAsync(id);
            if (!loadImageResult.IsSuccessful)
            {
                return GetImageLoadErrorResult(loadImageResult.Error);
            }
            var image = PhysicalFile(loadImageResult.FilePath, loadImageResult.FileMediaType);

            return image;
        }

        [HttpGet("users/current")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> GetForCurrentUser()
        {
            var currentUserIdentity = User.Identity;
            var loadImageResult = await _imageStorageService.LoadImageForCurrentUserAsync(currentUserIdentity);
            if (!loadImageResult.IsSuccessful)
            {
                return GetImageLoadErrorResult(loadImageResult.Error);
            }
            var image = PhysicalFile(loadImageResult.FilePath, loadImageResult.FileMediaType);

            return image;
        }

        [HttpPost]
        public async Task<ActionResult<SavedImageDataContract>> UploadImage(IFormFile image)
        {
            if (image == null)
            {
                return BadRequest();
            }
            var imageStream = image.OpenReadStream();
            var getImageSizeResult = _imageService.GetImageSize(imageStream);
            if (!getImageSizeResult.IsSuccessful)
            {
                return GetImageSaveErrorResult(getImageSizeResult.Error);
            }
            var saveImageResult = await _imageStorageService.SaveImageAsync(image.FileName, imageStream);
            if (!saveImageResult.IsSuccessful)
            {
                return GetImageSaveErrorResult(saveImageResult.Error);
            }
            var imageUrl = GetImageUrl(saveImageResult.ExternalId, getImageSizeResult.Width, getImageSizeResult.Height);
            var dataContract = new SavedImageDataContract
            {
                Url = imageUrl,
                ExternalId = saveImageResult.ExternalId
            };

            return dataContract;
        }


        private string GetImageUrl(string externalId, int width, int height)
        {
            return Url.Action(nameof(Get), "Images", new { id = externalId, width, height }, Request.Scheme);
        }

        private ObjectResult GetImageLoadErrorResult(LoadImageError error)
        {
            switch (error)
            {
                case LoadImageError.IncorrectExternalId:
                    {
                        var apiErrorDataContract = new ApiErrorDataContract(LoadImageApiErrorCodes.IncorrectExternalId);

                        return NotFound(apiErrorDataContract);
                    }
                case LoadImageError.ImageNotExists:
                    {
                        var apiErrorDataContract = new ApiErrorDataContract(GenericApiErrorCodes.UnknownError);

                        return StatusCode(500, apiErrorDataContract);
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(error), error, null);
            }
        }

        private ObjectResult GetImageSaveErrorResult(GetImageSizeError error)
        {
            switch (error)
            {
                case GetImageSizeError.InvalidImage:
                    {
                        var apiErrorDataContract = new ApiErrorDataContract(SaveImageApiErrorCodes.InvalidImage);

                        return BadRequest(apiErrorDataContract);
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(error), error, null);
            }
        }

        private ObjectResult GetImageSaveErrorResult(SaveImageError error)
        {
            var errorCode = GetImageSaveErrorCode(error);
            var apiErrorDataContract = new ApiErrorDataContract(errorCode);

            return BadRequest(apiErrorDataContract);
        }

        private static string GetImageSaveErrorCode(SaveImageError error)
        {
            switch (error)
            {
                case SaveImageError.EmptyFilePath:
                    return SaveImageApiErrorCodes.EmptyFilePath;
                case SaveImageError.TooBigImage:
                    return SaveImageApiErrorCodes.TooBigFile;
                case SaveImageError.NotAllowedMediaType:
                    return SaveImageApiErrorCodes.NotAllowedMediaType;
                default:
                    throw new ArgumentOutOfRangeException(nameof(error), error, null);
            }
        }
    }
}