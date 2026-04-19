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
    
}