namespace WebAppPassport.Services.ResponseModels;

public class PassportListItem
{
    public required string Name { get; set; }
    public required string IsoShortCode { get; set; }
    public required string Link { get; set; }
}

public class PassportDetail
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
    public List<CountrySummary> Countries { get; set; } = new();
    public Dictionary<string, List<CountrySummary>> Destinations { get; set; } = new();
}

public class PassportRankInfo
{
    public required string IsoShortCode { get; set; }
    public int MobilityScore { get; set; }
    public int WorldRank { get; set; }
    public required string Link { get; set; }
}
