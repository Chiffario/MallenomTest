using MallenomTest.Contracts;
using MallenomTest.Database;
using MallenomTest.Database.Models;
using MallenomTest.Services.Interfaces;
using MallenomTest.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace MallenomTest.Services;

public class ImagesService : IImagesService
{
    private readonly DatabaseContext _databaseContext;
    private readonly ILogger<ImagesService> _logger;

    public ImagesService(DatabaseContext context, ILogger<ImagesService> logger)
    {
        _databaseContext = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all the images from the database and return them in a list
    /// </summary>
    /// <returns>List of <see cref="ImageResponse"/></returns>
    public Task<List<ImageResponse>> GetAll()
    {
        return
            _databaseContext.Images.AsNoTracking()
            .Select(i =>
                new ImageResponse
                {
                    Id = i.Id,
                    Name = i.Name,
                    FileType = i.FileType,
                    Base64EncodedImage = Convert.ToBase64String(i.Data)
                }
            )
            .ToListAsync();
    }

    /// <summary>
    /// Adds an image to the database and to the filesystem.
    /// </summary>
    /// <param name="imageRequest">Image to add</param>
    public async Task Add(ImageRequest imageRequest)
    {
        _databaseContext.Images.Add(new ImageModel
        {
            Name = imageRequest.Name,
            FileType = imageRequest.FileType,
            Data = Convert.FromBase64String(imageRequest.Base64EncodedImage)
        });
        await _databaseContext.SaveChangesAsync();
    }

    /// <summary>
    /// Adds an image to the database and to the filesystem.
    /// Does so in a transactional way so a failed File creation/deletion shouldn't result in empty database entries 
    /// </summary>
    /// <param name="id">ID of image to update</param>
    /// <param name="imageRequest">Image to replace the existing one with</param>
    /// <exception cref="ArgumentOutOfRangeException">No image found with ID used</exception>
    public async Task Update(int id, ImageRequest imageRequest)
    {
        // Get the image to update
        var image = await _databaseContext.Images.FirstOrDefaultAsync(img => img.Id == id);

        if (image == null) throw new ArgumentOutOfRangeException(nameof(id));
        _logger.LogInformation($"Updated {image.Id} and changed name from {image.Name} -> {imageRequest.Name}");

        // Update the image with new info
        image.Name = imageRequest.Name;
        image.Data = Convert.FromBase64String(imageRequest.Base64EncodedImage);

        await _databaseContext.SaveChangesAsync();
    }

    /// <summary>
    /// Adds an image to the database.
    /// </summary>
    /// <param name="id">ID of image to delete</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown in case the ID wasn't in the DB or the respective file was not found</exception>
    public async Task Delete(int id)
    {
        var image = _databaseContext.Images.FirstOrDefault(img => img.Id == id);
        if (image == null)
        {
            throw new ArgumentOutOfRangeException();
        }
        _databaseContext.Images.Remove(image);
        await _databaseContext.SaveChangesAsync();
    }
}