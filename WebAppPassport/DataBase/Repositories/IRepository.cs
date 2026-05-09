using System.Linq.Expressions;
using WebAppPassport.DataBase.Models;


namespace WebAppPassport.DataBase.Repositories;

public interface IRepository
{
    public Task AddPassportCountryAsync(Country country, Passport passport);
    
    public Task AddPassportCountryRangeAsync(ICollection<Passport> passports, ICollection<Country> countries);
    
    public Task<ICollection<Country>?> GetAllCountriesExceptAsync(Country country);
    
    public Task<ICollection<Passport>?> GetAllPassportsAsync();
    
    public Task<ICollection<Country>?> GetAllCountriesAsync();
    
    public Task AddUserAsync(User user);
    
    public Task LinkPassportToUserAsync(Passport passport, User user);
    
    public Task LinkCountryToUserAsync(Country country, User user);
    
    public Task UpdateUserAsync(User user);
    
    public Task<ICollection<PassportCountryVisa>> GetAllDestinationsAsync();
    
    public Task AddDestinationAsync(PassportCountryVisa pcv);

    public Task AddDestinationsRangeAsync(ICollection<PassportCountryVisa> pcvs);

    public Task UpdateDestinationsRangeAsync(ICollection<PassportCountryVisa> pcvs);

    public Task<Passport?> GetPassportWithDestinationsAsync(string isoShortCode);

    public Task<ICollection<Passport>> GetAllPassportsWithCountriesAndDestinationsAsync();

    public Task<ICollection<Passport>> GetAllPassportsOrderedByRankAsync();

    public Task<Country?> GetCountryWithDestinationsAsync(string isoShortCode);

    public Task<ICollection<Country>> GetAllCountriesWithPassportAsync();

    public Task SaveChangesAsync();

    public Task<User?> GetUserByUsernameAsync(string username);

    public Task<User?> GetUserWithPassportsAndDestinationsAsync(string username);

    public Task<User?> GetUserWithPassportsAndCountriesAsync(string username);

    public Task<ICollection<Passport>> GetPassportsByIsosAsync(IEnumerable<string> isos);

    public Task LinkPassportsToUserAsync(IEnumerable<string> isos, string username);

    public Task LinkCountriesToUserAsync(IEnumerable<string> isos, string username);

    public Task UnlinkPassportsFromUserAsync(string username, IEnumerable<string> isos);

    public Task UnlinkCountriesFromUserAsync(string username, IEnumerable<string> isos);

    public Task UpdateUserVisibilityAsync(string username, bool? showPassports, bool? showCountries);

}