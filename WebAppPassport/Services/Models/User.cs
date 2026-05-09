using System.ComponentModel.DataAnnotations;

namespace WebAppPassport.Services.Models;

public class User
{
    [MaxLength(20)]
    public required string Username { get; set; }

    public required string HashedPassword { get; set; }

    [MaxLength(2)]
    public string? MotherlandIso { get; set; }

    public bool ShowPassports { get; set; } = true;
    public bool ShowCountries { get; set; } = true;

    public ICollection<Passport>? Passports { get; set; }
    public ICollection<Country>? Countries { get; set; }
}
