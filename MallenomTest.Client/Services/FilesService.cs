using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using MallenomTest.Client.Services.Interfaces;

namespace MallenomTest.Client.Services;

/// <summary>
/// Simple service for filesystem interactions
/// </summary>
public class FilesService : IFilesService
{
    private readonly Window _target;

    public FilesService(Window target)
    {
        _target = target;
    }

    /// <summary>
    /// Opens system file picker to select a file
    /// </summary>
    /// <returns></returns>
    public async Task<IStorageFile?> OpenFileAsync()
    {
        IReadOnlyList<IStorageFile?> files = await _target.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Select an image",
            AllowMultiple = false,
            FileTypeFilter = [FilePickerFileTypes.ImageAll]
        });

        return files.Count == 0 ? null : files[0];
    }
}