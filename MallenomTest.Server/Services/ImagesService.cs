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
    public List<ImageContract>? GetAll()
    {
        List<ImageContract>? images = 
            _databaseContext.Images.AsNoTracking()
            .Select(i => new ImageContract
            {
                Id = i.Id,
                Name = i.Name,
                Bytes = File.ReadAllBytes(Path.Combine(_webHostEnvironment.ContentRootPath, i.Id.ToString()))
            })
            .Take(50)
            .ToList();
        return images;
    }

    public Task Add(ImageJson imageJson)
    {
        var image = new ImageModel
        {
            Name = imageJson.Name,
            MimeType = imageJson.ImageType,
        };
        _databaseContext.Images.Add(image);

        byte[] imageBytes = Convert.FromBase64String(imageJson.base64EncodedImage);

        string imagePath = Path.Combine(_webHostEnvironment.ContentRootPath, image.Id + "." + image.MimeType);
        File.WriteAllBytes(imagePath, imageBytes);
        // TODO: Add a real return type lol
        return _databaseContext.SaveChangesAsync();
    }

    public Task Update(int id, ImageJson imageJson)
    {
        var img =  _databaseContext.Images.First(img => img.Id == id);
        string imagePath = Path.Combine(_webHostEnvironment.ContentRootPath, img.Id + "." + img.MimeType);
        File.Delete(imagePath);
        string newImagePath = Path.Combine(_webHostEnvironment.ContentRootPath, img.Id + "." + imageJson.ImageType);
        byte[] imageBytes = Convert.FromBase64String(imageJson.base64EncodedImage);
        img.Name = imageJson.Name;
        _databaseContext.SaveChanges();
        return File.WriteAllBytesAsync(newImagePath, imageBytes);
    }

    public Task Delete(int id)
    {
        var toDelete = _databaseContext.Images.First(img => img.Id == id);
        _databaseContext.Images.Remove(toDelete);
        return _databaseContext.SaveChangesAsync();
    }
}