using System.ComponentModel.DataAnnotations;

namespace WebAppPassport.DataBase.Models;

public class Country
{
    public Guid Id { get; set; }
    
    [MaxLength(100)]
    public required string Name { get; set; }
    
    [MaxLength(2)]
    public required string IsoShortCode { get; set; }
    
    public long? Population { get; set; }
    public bool? DualCitizenshipAllowed { get; set; }
    public long? PassportValidityRequirementInSeconds { get; set; }
    
    public required Passport Passport { get; set; }
    
    public required ICollection<PassportCountryVisa> PassportCountryVisas { get; set; }
    public ICollection<User>? Users { get; set; }
}