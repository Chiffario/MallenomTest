using MallenomTest.Contracts;
using MallenomTest.Services.Interfaces;
using MallenomTest.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace MallenomTest.Controllers
{
    [ApiController]
    [Route("api/images")]
    public class ApiController(ILogger<ApiController> logger, IImagesService imagesService) : ControllerBase
    {
        private readonly ILogger<ApiController> _logger = logger;
        private readonly IImagesService _imagesService = imagesService;
        
        [Route("all")]
        [HttpGet]
        public List<ImageResponse> GetAllImages()
        {
            return _imagesService.GetAll() ?? new List<ImageResponse>();
        }

        [Route("add")]
        [HttpPost]
        public Task<IActionResult> Add([FromBody] ImageRequest image)
        {
            try
            {
                _imagesService.Add(image);
            }
            catch (Exception e) 
            {
                _logger.LogError($"Failed to add a file: {e}");
                return Task.FromResult<IActionResult>(new StatusCodeResult(StatusCodes.Status500InternalServerError));
            }
            return Task.FromResult<IActionResult>(Ok());
        } 

        [Route("update/{id}")]
        [HttpPut]
        public async Task<IActionResult> Update(int id, [FromBody] ImageRequest image)
        {
            try
            {
                await _imagesService.Update(id, image);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to update file: {e}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }

        [Route("delete/{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _imagesService.Delete(id);
            }
            catch
            {
                _logger.LogError("Failed to delete file");
                return NotFound();
            }
            return Ok();
        }
    }
}