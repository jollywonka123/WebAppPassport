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

    public bool ShowPassports { get; set; } = true;
    public bool ShowCountries { get; set; } = true;

    public ICollection<Passport> Passports { get; set; } = new List<Passport>();
    public ICollection<Country> Countries { get; set; } = new List<Country>();
}
