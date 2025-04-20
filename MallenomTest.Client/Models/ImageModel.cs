using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace MallenomTest.Client.Models;

/// <summary>
/// Data model for in-memory storage of images for displaying via <see cref="DataGrid"/>
/// </summary>
public class ImageModel
{
    public ImageModel(string name, Bitmap imageBytes, int id)
    {
        Name = name;
        ImageBytes = imageBytes;
        Id = id;
    }

    /// <summary>
    /// ID of the image as provided by the backend
    /// </summary>
    public int Id { get; }
    /// <summary>
    /// Name of the image file
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// <see cref="Bitmap"/> representation of the image
    /// </summary>
    public Bitmap ImageBytes { get; set; }
}