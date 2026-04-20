using System.Globalization;
using CsvHelper;
using WebAppPassport.DataBase.StaticData.ModelsForStatic;
using WebAppPassport.Services.Models;

namespace WebAppPassport.Services.StaticDataServices;

public class StaticService: IStaticService
{
    private ICollection<PopulationModel>  _populations;
    
    private ICollection<LossModelByDualCitizenship> _losses;
    
    public StaticService()
    {
        _losses = GetAllLosses();
        _populations = GetAllPopulations();
    }

    public void EnrichCountry(Country country)
    {
        foreach (var population in _populations)
        {
            if (population.CountryName == country.Name)
            {
                country.Population = population.Population;
                country.Area = population.LandAreaDoubledKm;
                break;
            }
        }

        foreach (var loss in _losses)
        {
            if (loss.CountryName == country.Name)
            {
                country.DualCitizenshipAllowed = loss.LawRef != "No provision";
                break;
            }
        }
    }
    
    
    private ICollection<PopulationModel> GetAllPopulations()
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Services", "StaticDataServices", "StaticData", "population.csv");
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<PopulationModel>().ToList();
        return records;
    }
    
    private ICollection<LossModelByDualCitizenship> GetAllLosses() 
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Services", "StaticDataServices", "StaticData", "models.csv");
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<LossModelByDualCitizenship>().ToList();
        return records;
    }
}