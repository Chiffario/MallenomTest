using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace MallenomTest.Database.Models;

public class ImageModel
{
    [Key]
    public int Id { get; set; }
    
    // TODO: This kind of needs validation for storage reasons
    public required string Name { get; set; }
    
    public required string MimeType { get; set; }
}