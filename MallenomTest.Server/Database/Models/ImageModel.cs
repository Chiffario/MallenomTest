using System.ComponentModel.DataAnnotations;

namespace MallenomTest.Database.Models;

public class ImageModel
{
    [Key]
    public int Id { get; set; }
    public required string Name { get; set; }
    
    public required string FileType { get; set; }
    
    public required byte[] Data { get; set; }
}