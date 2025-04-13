using System.ComponentModel.DataAnnotations;

namespace MallenomTest.Database.Models;

public class ImageModel
{
    [Key]
    public int Id { get; set; }
    
    // TODO: This kind of needs validation for storage reasons
    public required string Name { get; set; }
    
    public required string FileType { get; set; }

    public string CreateFilePath()
    {
        return $"{Id}{FileType}";
    }
}