using WebAppPassport.Converters;
using WebAppPassport.DataBase.Repositories;
using WebAppPassport.Services.Models;

namespace WebAppPassport.Services.CountryService;

public class CountryService(IRepository repository) : ICountryService
{
    public async Task<ICollection<Country>> GetAllCountriesAsync()
    {
        var efCountries = await repository.GetAllCountriesAsync();
        return (efCountries ?? []).ToServiceEntity();
    }

    public async Task<Country?> GetCountryDetailAsync(string isoShortCode)
    {
        var efCountry = await repository.GetCountryWithDestinationsAsync(isoShortCode);
        return efCountry?.ToServiceEntity();
    }
}
