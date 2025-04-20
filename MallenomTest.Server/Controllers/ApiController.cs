using MallenomTest.Contracts;
using MallenomTest.Services.Interfaces;
using MallenomTest.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace MallenomTest.Controllers
{
    [ApiController]
    [Route("api/images")]
    public class ApiController : ControllerBase
    {
        private readonly ILogger<ApiController> _logger;
        private readonly IImagesService _imagesService;

        public ApiController(ILogger<ApiController> logger, IImagesService imagesService)
        {
            _logger = logger;
            _imagesService = imagesService;
        }

        /// <summary>
        /// Gets all the images stored in the database. Returns an empty list if none were found
        /// </summary>
        /// <returns>List of <see cref="ImageResponse"/></returns>
        [Route("all")]
        [HttpGet]
        [ProducesResponseType<List<ImageResponse>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await _imagesService.GetAll();
            if (images.Count == 0)
            {
                _logger.LogError("Error: No images found");
                images = new List<ImageResponse>();
            }

            return Ok(images);
        }

        /// <summary>
        /// Endpoint for image addition
        /// </summary>
        /// <param name="image">Image to add</param>
        [Route("add")]
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Add([FromBody] ImageRequest image)
        {
            try
            {
                await _imagesService.Add(image);
            }
            catch (Exception e) 
            {
                _logger.LogCritical($"Unexpected error: {e}");
                return new StatusCodeResult(500);
            }
            return Ok();
        } 
        

        /// <summary>
        /// Endpoint for image updates
        /// </summary>
        /// <param name="id">ID of image to update</param>
        /// <param name="image">Image to replace the stored one with</param>
        [Route("update/{id}")]
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Update(int id, [FromBody] ImageRequest image)
        {
            try
            {
                await _imagesService.Update(id, image);
            }
            catch (ArgumentOutOfRangeException e)
            {
                _logger.LogError($"Error: {e.Message}");
                return Problem(
                    detail: $"Update failed: image with ID {id} was not found",
                    statusCode: 404);
            }
            catch (Exception e)
            {
                _logger.LogCritical($"Unexpected error: {e}");
                return new StatusCodeResult(500);
            }
            return Ok();
        }

        /// <summary>
        /// Endpoint for image deletion
        /// </summary>
        /// <param name="id">ID of image to delete</param>
        [Route("delete/{id}")]
        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _imagesService.Delete(id);
            }
            catch (ArgumentOutOfRangeException e)
            {
                _logger.LogError($"Error: {e.Message}");
                return Problem(
                    detail: $"Deletion failed: image with ID {id} was not found",
                    statusCode: 404);
            }
            catch (Exception e)
            {
                _logger.LogCritical($"Unexpected error: {e}");
                return new StatusCodeResult(500);
            }
            return Ok();
        }
    }
}