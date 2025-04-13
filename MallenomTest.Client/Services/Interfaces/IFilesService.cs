using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace MallenomTest.Client.Services.Interfaces;

public interface IFilesService
{
    public Task<IStorageFile> OpenFileAsync();
}