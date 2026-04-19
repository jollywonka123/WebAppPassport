using CsvHelper.Configuration.Attributes;

namespace WebAppPassport.Services.BackgroundServices.StaticData.ModelsForStatic;

public class PopulationModel
{
    [Name("Country (or dependency)")]
    public required string CountryName { get; set; }
    
    [Name("Population 2025")]
    public long Population { get; set; }
    
    [Name("Land Area (Km²)")]
    public double LandAreaDoubledKm { get; set; }
}