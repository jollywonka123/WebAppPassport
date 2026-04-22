namespace WebAppPassport.Services.ResponseModels;

public class CountryListItem
{
    public required string Name { get; set; }
    public required string IsoShortCode { get; set; }
    public required string Link { get; set; }
}

public class CountryDetail
{
    public required string Name { get; set; }
    public required string IsoShortCode { get; set; }
    public long? Population { get; set; }
    public double? Area { get; set; }
    public bool? DualCitizenshipAllowed { get; set; }
    public Dictionary<string, List<CountrySummary>> Destinations { get; set; } = new();
}
