using System.Buffers.Text;
using MallenomTest.Contracts;
using MallenomTest.Database;
using MallenomTest.Database.Models;
using MallenomTest.Services.Interfaces;
using MallenomTest.Services.Models;
using Microsoft.AspNetCore.Mvc;
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
    public List<ImageResponse>? GetAll()
    {
        List<ImageResponse>? images = 
            _databaseContext.Images.AsNoTracking()
            .Select(i =>
                new ImageResponse
                {
                    Id = i.Id,
                    Name = i.Name,
                    FileType = i.FileType,
                    Base64EncodedImage = File.ReadAllBytes(Path.Combine(_webHostEnvironment.ContentRootPath, i.CreateFilePath()))
                }
            )
            .Take(50)
            .ToList();
        return images;
    }

    public void Add(ImageRequest imageRequest)
    {
        var image = new ImageModel
        {
            Name = imageRequest.Name,
            FileType = imageRequest.FileType,
        };
        _databaseContext.Images.Add(image);

        _databaseContext.SaveChanges();
        byte[] imageBytes = Convert.FromBase64String(imageRequest.Base64EncodedImage);

        string imagePath = Path.Combine(_webHostEnvironment.ContentRootPath, image.CreateFilePath());
        File.WriteAllBytes(imagePath, imageBytes);
        // TODO: Add a real return type lol
         
    }

    public Task Update(int id, ImageRequest imageRequest)
    {
        var img =  _databaseContext.Images.First(img => img.Id == id);
        string imagePath = Path.Combine(_webHostEnvironment.ContentRootPath, img.CreateFilePath());
        File.Delete(imagePath);
        string newImagePath = Path.Combine(_webHostEnvironment.ContentRootPath, img.CreateFilePath());
        byte[] imageBytes = Convert.FromBase64String(imageRequest.Base64EncodedImage);
        img.Name = imageRequest.Name;
        _logger.LogInformation($"Updated {img.Id} and changed name from {img.Name} -> {imageRequest.Name}");
        _databaseContext.SaveChanges();
        return File.WriteAllBytesAsync(newImagePath, imageBytes);
    }

    public Task Delete(int id)
    {
        var toDelete = _databaseContext.Images.FirstOrDefault(img => img.Id == id);
        if (toDelete is null)
        {
            _logger.LogError("ID not found in db");
            throw new DirectoryNotFoundException();
        }
        string imagePath = Path.Combine(_webHostEnvironment.ContentRootPath, toDelete.CreateFilePath());
        File.Delete(imagePath);
        _logger.LogInformation($"Deleting at path {imagePath}");
        _databaseContext.Images.Remove(toDelete);
        return _databaseContext.SaveChangesAsync();
    }
}