using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace MallenomTest.Client.Models;

public class ImageModel
{
    public ImageModel(string name, Bitmap imageBytes, int id)
    {
        Name = name;
        ImageBytes = imageBytes;
        Id = id;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    
    public Bitmap ImageBytes { get; set; }
}