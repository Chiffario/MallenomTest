namespace MallenomTest.Contracts;

public class ImageResponse
{
    public int Id { get; set; }
    
    public required string Name { get; set; }
    
    public required string FileType { get; set; }
    
    public required byte[] Base64EncodedImage { get; set; }
}