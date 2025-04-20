namespace MallenomTest.Services.Models;

public class ImageRequest
{
    /// <summary>
    /// Name of the image
    /// </summary>
    public required string Name { get; set; }
    
    /// <summary>
    /// Canonical file extension of the image
    /// </summary>
    public required string FileType { get; set; }
    
    /// <summary>
    /// Image encoded as base64
    /// </summary>
    public required string Base64EncodedImage { get; set; }
}