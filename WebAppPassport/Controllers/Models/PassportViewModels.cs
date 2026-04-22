using WebAppPassport.Controllers.Models;

namespace WebAppPassport.Controllers.Models;

public class PassportListItemViewModel
{
    public required string Name { get; set; }
    public required string IsoShortCode { get; set; }
    public required string Link { get; set; }
}

public class PassportDetailViewModel
{
    public required string Name { get; set; }
    public required string IsoShortCode { get; set; }
    public int MobilityScore { get; set; }
    public int WorldRank { get; set; }
    public int VisaFreeCount { get; set; }
    public int VisaOnArrivalCount { get; set; }
    public int EVisaCount { get; set; }
    public int RequiredVisaCount { get; set; }
    public long? TotalPopulation { get; set; }
    public List<CountrySummaryViewModel> Countries { get; set; } = new();
    public Dictionary<string, List<CountrySummaryViewModel>> Destinations { get; set; } = new();
}

public class PassportRankInfoViewModel
{
    public required string IsoShortCode { get; set; }
    public int MobilityScore { get; set; }
    public int WorldRank { get; set; }
    public required string Link { get; set; }
}
