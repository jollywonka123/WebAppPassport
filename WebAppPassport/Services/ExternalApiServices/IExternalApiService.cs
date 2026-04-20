using WebAppPassport.Services.Models;

namespace WebAppPassport.Services.ExternalApiServices;

public interface IExternalApiService
{
    public Task<(ICollection<Country>, ICollection<Passport>)> GetAllCountriesAndPassportsAsync();

    public Task<ICollection<PassportCountryVisa>> GetAllDestinationsByPassportAsync(Passport passport);
    
}