namespace MallenomTest.Services.Models;

public class ImageRequest
{
    
    public required string Name { get; set; }
    public required string FileType { get; set; }
    
    public required string Base64EncodedImage { get; set; }
}