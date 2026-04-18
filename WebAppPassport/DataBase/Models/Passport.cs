using System.ComponentModel.DataAnnotations;

namespace WebAppPassport.DataBase.Models;

public class Passport
{
    public Guid Id { get; set; }
    
    [MaxLength(100)]
    public required string Name { get; set; }
    
    [MaxLength(2)]
    public required string IsoShortCode { get; set; }
    public required Country Country { get; set; }
    public Guid CountryId { get; set; }
    
    public required ICollection<PassportCountryVisa> PassportCountryVisas { get; set; }
    
    public required ICollection<User> Users { get; set; }
}