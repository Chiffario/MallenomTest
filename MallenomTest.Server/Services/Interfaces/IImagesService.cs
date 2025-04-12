using MallenomTest.Contracts;
using MallenomTest.Services.Models;

namespace MallenomTest.Services.Interfaces;

public interface IImagesService
{
    List<ImageContract>? GetAll();
    Task Add(ImageJson imageJson);
    Task Update(int id, ImageJson imageJson);
    Task Delete(int id);
}