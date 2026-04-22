using System.ComponentModel.DataAnnotations;

namespace WebAppPassport.DataBase.Models;

public class Passport
{
    public Guid Id { get; set; }

    [MaxLength(100)]
    public required string Name { get; set; }

    [MaxLength(2)]
    public required string IsoShortCode { get; set; }

    public ICollection<Country> Countries { get; set; } = new List<Country>();
    public ICollection<PassportCountryVisa> PassportCountryVisas { get; set; } = new List<PassportCountryVisa>();
    public ICollection<User> Users { get; set; } = new List<User>();

    public int MobilityScore { get; set; }
    public int WorldRank { get; set; }
    public int VisaFreeCount { get; set; }
    public int VisaOnArrivalCount { get; set; }
    public int EVisaCount { get; set; }
    public int RequiredVisaCount { get; set; }
    public long? TotalPopulation { get; set; }
}
