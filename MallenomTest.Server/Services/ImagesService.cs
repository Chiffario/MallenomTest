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
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<ImagesService> _logger;

    public ImagesService(DatabaseContext context, ILogger<ImagesService> logger, IWebHostEnvironment environment)
    {
        _databaseContext = context;
        _webHostEnvironment = environment;
        _logger = logger;
    }
    
    /// <summary>
    /// Get all the images from the database and return them in a list
    /// </summary>
    /// <returns>List of <see cref="ImageResponse"/></returns>
    public List<ImageResponse> GetAll()
    {
        List<ImageResponse> images = 
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
            .ToList();
        return images;
    }

    /// <summary>
    /// Adds an image to the database and to the filesystem.
    /// Does so in a transactional way so a failed File creation shouldn't result in empty database entries 
    /// </summary>
    /// <param name="imageRequest">Image to add</param>
    public void Add(ImageRequest imageRequest)
    {
        var transaction = _databaseContext.Database.BeginTransaction();
        
        var image = new ImageModel
        {
            Name = imageRequest.Name,
            FileType = imageRequest.FileType,
            Data = Convert.FromBase64String(imageRequest.Base64EncodedImage)
        };
        _databaseContext.Images.Add(image);
        _databaseContext.SaveChanges();
        
        transaction.Commit();
    }

    /// <summary>
    /// Adds an image to the database and to the filesystem.
    /// Does so in a transactional way so a failed File creation/deletion shouldn't result in empty database entries 
    /// </summary>
    /// <param name="id">ID of image to update</param>
    /// <param name="imageRequest">Image to replace the existing one with</param>
    public Task Update(int id, ImageRequest imageRequest)
    {
        var transaction = _databaseContext.Database.BeginTransaction();
        
        var image =  _databaseContext.Images.First(img => img.Id == id);
        
        byte[] imageBytes = Convert.FromBase64String(imageRequest.Base64EncodedImage);
        _logger.LogInformation($"Updated {image.Id} and changed name from {image.Name} -> {imageRequest.Name}");
        
        image.Name = imageRequest.Name;
        image.Data = imageBytes;
        _databaseContext.SaveChanges();
        
        return transaction.CommitAsync();
    }

    /// <summary>
    /// Adds an image to the database and to the filesystem.
    /// Does so in a transactional way so a failed File creation/deletion shouldn't result in empty database entries 
    /// </summary>
    /// <param name="id">ID of image to delete</param>
    /// <exception cref="FileNotFoundException">Thrown in case the ID wasn't in the DB or the respective file was not found</exception>
    public Task Delete(int id)
    {
        var transaction = _databaseContext.Database.BeginTransaction();

        var image = _databaseContext.Images.FirstOrDefault(img => img.Id == id);
        if (image != null) _databaseContext.Images.Remove(image);
        _databaseContext.SaveChanges();
        
        return transaction.CommitAsync();

    }
}