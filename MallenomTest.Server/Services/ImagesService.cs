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
                    Base64EncodedImage = File.ReadAllBytes(BuildImagePath(_webHostEnvironment.ContentRootPath, i))
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
        };
        _databaseContext.Images.Add(image);
        _databaseContext.SaveChanges();

        string imagePath = BuildImagePath(_webHostEnvironment.ContentRootPath, image);
        
        byte[] imageBytes = Convert.FromBase64String(imageRequest.Base64EncodedImage);
        File.WriteAllBytes(imagePath, imageBytes);
        
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
        
        string imagePath = BuildImagePath(_webHostEnvironment.ContentRootPath, image);
        File.Delete(imagePath);
        
        byte[] imageBytes = Convert.FromBase64String(imageRequest.Base64EncodedImage);
        _logger.LogInformation($"Updated {image.Id} and changed name from {image.Name} -> {imageRequest.Name}");
        
        image.Name = imageRequest.Name;
        _databaseContext.SaveChanges();
        
        File.WriteAllBytes(imagePath, imageBytes);
        
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
        if (image is null)
        {
            _logger.LogError("ID not found in db");
            throw new FileNotFoundException();
        }
        string imagePath = BuildImagePath(_webHostEnvironment.ContentRootPath, image);
        File.Delete(imagePath);
        _logger.LogInformation($"Deleting at path {imagePath}");
        _databaseContext.Images.Remove(image);
        _databaseContext.SaveChanges();
        
        return transaction.CommitAsync();

    }

    /// <summary>
    /// Creates a file path for filesystem manipulation
    /// </summary>
    /// <param name="contentRootPath">Web host's content root</param>
    /// <param name="imageModel">Image to base the path one</param>
    /// <returns>New path, usually looking like `/app/storage/id.type`</returns>
    private static string BuildImagePath(string contentRootPath, ImageModel imageModel)
    {
        return Path.Combine(contentRootPath, "storage", imageModel.CreateFilePath());
    }
}