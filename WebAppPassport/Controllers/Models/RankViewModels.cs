namespace WebAppPassport.Controllers.Models;

public class RankViewModel
{
    public List<RankPassportViewModel> Passports { get; set; } = new();
    public List<RankCountryViewModel> Countries { get; set; } = new();
}

public class RankPassportViewModel
{
    public required string Name { get; set; }
    public required string IsoShortCode { get; set; }
    public int MobilityScore { get; set; }
    public int WorldRank { get; set; }
}

public class RankCountryViewModel
{
    public required string Name { get; set; }
    public required string IsoShortCode { get; set; }
    public int MobilityScore { get; set; }
    public int WorldRank { get; set; }
}
