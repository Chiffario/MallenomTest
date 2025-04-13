using MallenomTest.Contracts;
using MallenomTest.Services.Models;

namespace MallenomTest.Services.Interfaces;

public interface IImagesService
{
    List<ImageResponse>? GetAll();
    void Add(ImageRequest imageRequest);
    Task Update(int id, ImageRequest imageRequest);
    Task Delete(int id);
}