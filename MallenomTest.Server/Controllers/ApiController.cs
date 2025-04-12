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
        public List<ImageContract> GetAllImages()
        {
            return _imagesService.GetAll() ?? new List<ImageContract>();
        }

        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ImageJson image)
        {
            await _imagesService.Add(image);
            return Ok();
        } 

        [Route("update/{id}")]
        [HttpPut]
        public async Task<IActionResult> Update(int id, [FromBody] ImageJson image)
        {
            await _imagesService.Update(id, image);
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
            catch (Exception e)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}