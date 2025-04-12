namespace MallenomTest.Contracts;

public class ImageContract
{
    public int Id { get; set; }
    
    public required string Name { get; set; }
    
    public required byte[] Bytes { get; set; }
}