using System.ComponentModel.DataAnnotations;

namespace WebAppPassport.DataBase.Models;

public class User
{
    public Guid Id { get; set; }
    
    [MaxLength(20)]
    public required string Username { get; set; }
    
    public required string HashedPassword { get; set; }
    
    [MaxLength(2)]
    public string? MotherlandIso { get; set; }
    
    public ICollection<Passport>? Passports { get; set; }
    public ICollection<Country>? Countries { get; set; }
}