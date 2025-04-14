namespace MallenomTest.Contracts;

public class ImageResponse
{
    /// <summary>
    /// Database ID of the image
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Canonical file name of the image. Includes file extension
    /// </summary>
    public required string Name { get; set; }
    
    /// <summary>
    /// Canonical file extension of the image
    /// </summary>
    public required string FileType { get; set; }
    
    /// <summary>
    /// Image encoded as base64
    /// </summary>
    public required byte[] Base64EncodedImage { get; set; }
}