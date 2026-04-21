using System.ComponentModel.DataAnnotations;

namespace WebAppPassport.DataBase.Models;

public class Passport
{
    public Guid Id { get; set; }
    
    [MaxLength(100)]
    public required string Name { get; set; }
    
    [MaxLength(2)]
    public required string IsoShortCode { get; set; }
    public ICollection<Country>? Countries { get; set; }
    
    
    public ICollection<PassportCountryVisa>? PassportCountryVisas { get; set; }
    
    public ICollection<User>? Users { get; set; }
}