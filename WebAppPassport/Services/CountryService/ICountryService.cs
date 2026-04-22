using WebAppPassport.Services.Models;

namespace WebAppPassport.Services.CountryService;

public interface ICountryService
{
    Task<ICollection<Country>> GetAllCountriesAsync();

    Task<Country?> GetCountryDetailAsync(string isoShortCode);
}
