using System.ComponentModel.DataAnnotations;

namespace WebAppPassport.Services.ServiceModels;

public class User
{
    [MaxLength(20)]
    public required string Username { get; set; }
    
    public required string HashedPassword { get; set; }
    
    [MaxLength(2)]
    public string? MotherlandIso { get; set; }
    
}