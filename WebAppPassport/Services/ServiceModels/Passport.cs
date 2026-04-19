using System.ComponentModel.DataAnnotations;

namespace WebAppPassport.Services.ServiceModels;

public class Passport
{
    
    [MaxLength(100)]
    public required string Name { get; set; }
    
    [MaxLength(2)]
    public required string IsoShortCode { get; set; }
    public Country? Country { get; set; }
    
}