using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsPortal.Application.Services;
using NewsPortal.Contracts;
using NewsPortal.Core.Models;

namespace NewsPortal.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class ImagesController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImagesController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ImageResponse>>> GetImages()
        {
            var images = await _imageService.GetAllImages();
            var response = images.Select(n => new ImageResponse(n.Id, n.Url));
            return Ok(response);
        }

        [HttpGet("slider")]
        public async Task<ActionResult<List<ImageResponse>>> GetSliderImages()
        {
            var images = await _imageService.GetAllImages();
            var response = images.Select(n => new ImageResponse(n.Id, n.Url));
            return Ok(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Guid>> CreateImage([FromBody] ImageRequest request)
        {
            var image = Image.Create(
                Guid.NewGuid(),
                request.Url
                );
            var imageId = await _imageService.CreateImage(image);
            return Ok(imageId);
        }

        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<ActionResult<Guid>> UpdateImage(Guid id, [FromBody] ImageRequest request)
        {
            var imageId = await _imageService.UpdateImage(id, request.Url);
            return Ok(imageId);
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<ActionResult<Guid>> DeleteImage(Guid id)
        {
            return Ok(await _imageService.DeleteImage(id));
        }
    }
}