using System.ComponentModel.DataAnnotations;

namespace WebAppPassport.Services.Models;

public class Country
{
    
    [MaxLength(100)]
    public required string Name { get; set; }
    
    [MaxLength(2)]
    public required string IsoShortCode { get; set; }
    
    public long? Population { get; set; }
    
    public double? Area { get; set; }
    public bool? DualCitizenshipAllowed { get; set; }
    public long? PassportValidityRequirementInSeconds { get; set; }
    
    public Passport? Passport { get; set; }
    
}