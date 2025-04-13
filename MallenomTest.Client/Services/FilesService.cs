using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using MallenomTest.Client.Services.Interfaces;

namespace MallenomTest.Client.Services;

public class FilesService : IFilesService
{
    private readonly Window _target;

    public FilesService(Window target)
    {
        _target = target;
    }
    public async Task<IStorageFile> OpenFileAsync()
    {
        var files = await _target.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Select an image",
            AllowMultiple = false,
            FileTypeFilter = [FilePickerFileTypes.ImageAll]
        });

        return files[0];
    }
}