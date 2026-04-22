using WebAppPassport.Services.ResponseModels;

namespace WebAppPassport.Services.CountryService;

public interface ICountryService
{
    Task<ICollection<CountryListItem>> GetAllCountriesAsync();

    Task<CountryDetail?> GetCountryDetailAsync(string isoShortCode);
}
