using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace MallenomTest.Client.Services.Interfaces;

public interface IFilesService
{
    /// <summary>
    /// Interface method to select a file
    /// </summary>
    /// <returns>Selected file</returns>
    public Task<IStorageFile?> OpenFileAsync();
}