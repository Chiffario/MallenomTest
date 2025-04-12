namespace MallenomTest.Services.Models;

public class ImageJson
{
    
    public required string Name { get; set; }
    public required string ImageType { get; set; }
    
    public required string base64EncodedImage { get; set; }
}