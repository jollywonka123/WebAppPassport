using CsvHelper.Configuration.Attributes;

namespace WebAppPassport.Services.BackgroundServices.StaticData.ModelsForStatic;

public class LossModelByDualCitizenship
{
    [Name("Country")]
    public required string CountryName { get; set; }
    
    [Name("Article in law")]
    public required string LawRef { get; set; }
}