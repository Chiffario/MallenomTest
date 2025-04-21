using System.ComponentModel.DataAnnotations;

namespace MallenomTest.Database.Models;

/// <summary>
/// Stored Image model
/// </summary>
public class ImageModel
{
    [Key]
    public int Id { get; set; }
    /// <summary>
    /// Name of the image file
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Canonical file extension of the image
    /// </summary>
    public required string FileType { get; set; }

    /// <summary>
    /// Raw image data as a byte blob
    /// </summary>
    public required byte[] Data { get; set; }
}