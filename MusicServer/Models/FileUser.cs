using System.ComponentModel.DataAnnotations;

namespace MusicServer.Models;

public class FileUser
{
    [Key]
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Path { get; set; }
    
    public string? Description { get; set; }
}