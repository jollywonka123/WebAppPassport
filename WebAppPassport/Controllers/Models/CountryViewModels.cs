namespace WebAppPassport.Controllers.Models;

public class CountrySummaryViewModel
{
    public required string Name { get; set; }
    public required string IsoShortCode { get; set; }
}

public class CountryListItemViewModel
{
    public required string Name { get; set; }
    public required string IsoShortCode { get; set; }
    public required string Link { get; set; }
}

public class CountryDetailViewModel
{
    public required string Name { get; set; }
    public required string IsoShortCode { get; set; }
    public long? Population { get; set; }
    public double? Area { get; set; }
    public bool? DualCitizenshipAllowed { get; set; }
    public Dictionary<string, List<CountrySummaryViewModel>> Destinations { get; set; } = new();
}
