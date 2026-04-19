using Microsoft.EntityFrameworkCore;
using WebAppPassport.DataBase.Models;

namespace WebAppPassport.DataBase.Repositories;

public class Repository(AppContext context): IRepository
{
    protected readonly AppContext Context = context;
    
    public async Task AddPassportCountryAsync(Country country, Passport passport)
    {
        passport.Country = country;
        await Context.Passports.AddAsync(passport);
        await Context.Countries.AddAsync(country);
        await Context.SaveChangesAsync();
    }

    public async Task AddPassportCountryRangeAsync(ICollection<Passport> passports, ICollection<Country> countries)
    {
        if (passports.Count != countries.Count)
            throw new ArgumentException("Passport count must be equal to count of country");
        for (int i = 0; i < passports.Count; i++)
        {
            passports.ElementAt(i).Country = countries.ElementAt(i);
        }
        await Context.Passports.AddRangeAsync(passports);
        await Context.Countries.AddRangeAsync(countries);
        await Context.SaveChangesAsync();
    }

    public async Task<ICollection<Country>?> GetAllCountriesExceptAsync(Country country)
    {
        return await Context.Countries.Where(x => x.IsoShortCode != country.IsoShortCode).ToListAsync();
    }

    public async Task<ICollection<Passport>?> GetAllPassportsAsync()
    {
        return await Context.Passports.Include(x => x.Country).ToListAsync();
    }

    public async Task<ICollection<Country>?> GetAllCountriesAsync()
    {
        return await Context.Countries.Include(x => x.Passport).ToListAsync();
    }

    public async Task AddUserAsync(User user)
    {
        await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();
    }

    public async Task LinkPassportToUserAsync(Passport passport, User user)
    {
        var foundPassport = await Context.Passports.FirstOrDefaultAsync(x => x.IsoShortCode == passport.IsoShortCode);
        if (foundPassport == null)
            throw new ArgumentException("While linking Passport to User, Passport was not found");
        var foundUser = await Context.Users.FirstOrDefaultAsync(x => x.Username == user.Username);
        
        if (foundUser == null)
            throw new ArgumentException("While linking Passport to User, User was not found");
        
        foundUser.Passports!.Add(foundPassport);
        await Context.SaveChangesAsync();
    }

    public async Task LinkCountryToUserAsync(Country country, User user)
    {
        var foundCountry = await Context.Countries.FirstOrDefaultAsync(x => x.IsoShortCode == country.IsoShortCode);
        if (foundCountry == null)
            throw new ArgumentException("While linking Country to User, Country was not found");
        var foundUser = await Context.Users.FirstOrDefaultAsync(x => x.Username == user.Username);
        
        if (foundUser == null)
            throw new ArgumentException("While linking Passport to User, User was not found");
        
        foundUser.Countries!.Add(foundCountry);
        await Context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        var foundUser = await Context.Users.FirstOrDefaultAsync(x => x.Username == user.Username);
        
        if (foundUser == null)
            throw new ArgumentException("While updating User, User was not found");
        
        foundUser.Username = user.Username;
        foundUser.HashedPassword = user.HashedPassword;
        foundUser.MotherlandIso = user.MotherlandIso;
        
        await Context.SaveChangesAsync();
    }

    public Task<ICollection<PassportCountryVisa>> GetAllDestinationsAsync()
    {
        throw new NotImplementedException();
    }

    public Task AddDestinationAsync(PassportCountryVisa pcv)
    {
        throw new NotImplementedException();
    }
}