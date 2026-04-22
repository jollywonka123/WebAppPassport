namespace WebAppPassport.Services.ResponseModels;

public class RankResponse
{
    public List<RankPassportItem> Passports { get; set; } = new();
    public List<RankCountryItem> Countries { get; set; } = new();
}

public class RankPassportItem
{
    public required string Name { get; set; }
    public required string IsoShortCode { get; set; }
    public int MobilityScore { get; set; }
    public int WorldRank { get; set; }
}

public class RankCountryItem
{
    public required string Name { get; set; }
    public required string IsoShortCode { get; set; }
    public int MobilityScore { get; set; }
    public int WorldRank { get; set; }
}
