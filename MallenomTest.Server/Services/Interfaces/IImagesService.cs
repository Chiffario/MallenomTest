using MallenomTest.Contracts;
using MallenomTest.Services.Models;

namespace MallenomTest.Services.Interfaces;

public interface IImagesService
{
    Task<List<ImageResponse>> GetAll();
    Task Add(ImageRequest imageRequest);
    Task Update(int id, ImageRequest imageRequest);
    Task Delete(int id);
}