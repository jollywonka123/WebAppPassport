using System.ComponentModel.DataAnnotations;

namespace WebAppPassport.Services.Models;

public class Passport
{
    [MaxLength(100)]
    public required string Name { get; set; }

    [MaxLength(2)]
    public required string IsoShortCode { get; set; }

    public Country? Country { get; set; }

    public ICollection<Country>? Countries { get; set; }

    public ICollection<PassportCountryVisa>? Destinations { get; set; }

    public int MobilityScore { get; set; }
    public int WorldRank { get; set; }
    public int VisaFreeCount { get; set; }
    public int VisaOnArrivalCount { get; set; }
    public int EtaCount { get; set; }
    public int RequiredVisaCount { get; set; }
    public long? TotalPopulation { get; set; }
}
