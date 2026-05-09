using System.Globalization;
using Microsoft.EntityFrameworkCore;
using WebAppPassport.DataBase.Models;
using WebAppPassport.DataBase.StaticData.ModelsForStatic;
using CsvHelper;

namespace WebAppPassport.DataBase.Repositories;

public class Repository(AppContext context) : IRepository
{
    protected readonly AppContext Context = context;

    public async Task AddPassportCountryAsync(Country country, Passport passport)
    {
        passport.Countries.Add(country);
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
            passports.ElementAt(i).Countries.Add(countries.ElementAt(i));
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
        return await Context.Passports.Include(x => x.Countries).ToListAsync();
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

        foundUser.Passports.Add(foundPassport);
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

        foundUser.Countries.Add(foundCountry);
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

    public async Task<ICollection<PassportCountryVisa>> GetAllDestinationsAsync()
    {
        return await Context.Destinations
            .Include(x => x.Country)
            .Include(x => x.Passport)
            .ToListAsync();
    }

    public async Task AddDestinationAsync(PassportCountryVisa pcv)
    {
        var foundPassport = await Context.Passports
            .FirstOrDefaultAsync(x => x.IsoShortCode == pcv.Passport.IsoShortCode);
        if (foundPassport == null)
            throw new ArgumentException("While adding Destination Passport was not found");

        var foundCountry = await Context.Countries
            .FirstOrDefaultAsync(x => x.IsoShortCode == pcv.Country.IsoShortCode);

        if (foundCountry == null)
            throw new ArgumentException("While adding Destination Country was not found");

        pcv.Passport = foundPassport;
        pcv.Country = foundCountry;

        await Context.Destinations.AddAsync(pcv);

        await Context.SaveChangesAsync();
    }

    public async Task AddDestinationsRangeAsync(ICollection<PassportCountryVisa> pcvs)
    {
        var allPassports = await Context.Passports
            .ToListAsync();

        var allCountries = await Context.Countries
            .ToListAsync();

        foreach (var pcv in pcvs)
        {
            foreach (var passport in allPassports)
            {
                if (pcv.Passport.IsoShortCode == passport.IsoShortCode)
                    pcv.Passport = passport;
            }

            foreach (var country in allCountries)
            {
                if (pcv.Country.IsoShortCode == country.IsoShortCode)
                    pcv.Country = country;
            }

            await Context.Destinations.AddRangeAsync(pcv);
            await Context.SaveChangesAsync();
        }
    }

    public async Task UpdateDestinationsRangeAsync(ICollection<PassportCountryVisa> pcvs)
    {
        var allPassports = await Context.Passports.ToListAsync();
        var allCountries = await Context.Countries.ToListAsync();

        var existing = await Context.Destinations
            .Include(x => x.Passport)
            .Include(x => x.Country)
            .ToListAsync();

        var toInsert = new List<PassportCountryVisa>();

        foreach (var pcv in pcvs)
        {
            var passport = allPassports.FirstOrDefault(p => p.IsoShortCode == pcv.Passport.IsoShortCode);
            if (passport == null) continue;

            var country = allCountries.FirstOrDefault(c => c.IsoShortCode == pcv.Country.IsoShortCode);
            if (country == null) continue;

            var found = existing.FirstOrDefault(x =>
                x.Passport.IsoShortCode == pcv.Passport.IsoShortCode &&
                x.Country.IsoShortCode == pcv.Country.IsoShortCode);

            if (found != null)
            {
                found.VisaType = pcv.VisaType;
            }
            else
            {
                toInsert.Add(new PassportCountryVisa
                {
                    Passport = passport,
                    Country = country,
                    VisaType = pcv.VisaType
                });
            }
        }

        if (toInsert.Count > 0)
            await Context.Destinations.AddRangeAsync(toInsert);

        await Context.SaveChangesAsync();
    }

    public async Task<Passport?> GetPassportWithDestinationsAsync(string isoShortCode)
    {
        return await Context.Passports
            .Include(p => p.Countries)
            .Include(p => p.PassportCountryVisas)
                .ThenInclude(pcv => pcv.Country)
            .FirstOrDefaultAsync(p => p.IsoShortCode == isoShortCode);
    }

    public async Task<ICollection<Passport>> GetAllPassportsWithCountriesAndDestinationsAsync()
    {
        return await Context.Passports
            .Include(p => p.Countries)
            .Include(p => p.PassportCountryVisas)
                .ThenInclude(pcv => pcv.Country)
            .ToListAsync();
    }

    public async Task<ICollection<Passport>> GetAllPassportsOrderedByRankAsync()
    {
        return await Context.Passports
            .OrderBy(p => p.WorldRank)
            .ToListAsync();
    }

    public async Task<Country?> GetCountryWithDestinationsAsync(string isoShortCode)
    {
        return await Context.Countries
            .Include(c => c.PassportCountryVisas)
                .ThenInclude(pcv => pcv.Passport)
                    .ThenInclude(p => p.Countries)
            .FirstOrDefaultAsync(c => c.IsoShortCode == isoShortCode);
    }

    public async Task<ICollection<Country>> GetAllCountriesWithPassportAsync()
    {
        return await Context.Countries
            .Include(c => c.Passport)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await Context.SaveChangesAsync();
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await Context.Users
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetUserWithPassportsAndDestinationsAsync(string username)
    {
        return await Context.Users
            .Include(u => u.Passports)
                .ThenInclude(p => p.PassportCountryVisas)
                    .ThenInclude(pcv => pcv.Country)
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetUserWithPassportsAndCountriesAsync(string username)
    {
        return await Context.Users
            .Include(u => u.Passports)
            .Include(u => u.Countries)
            .FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<ICollection<Passport>> GetPassportsByIsosAsync(IEnumerable<string> isos)
    {
        var isoList = isos.ToList();
        return await Context.Passports
            .Include(p => p.Countries)
            .Include(p => p.PassportCountryVisas)
                .ThenInclude(pcv => pcv.Country)
            .Where(p => isoList.Contains(p.IsoShortCode))
            .ToListAsync();
    }

    public async Task LinkPassportsToUserAsync(IEnumerable<string> isos, string username)
    {
        var isoList = isos.ToList();
        var user = await Context.Users
            .Include(u => u.Passports)
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null) throw new ArgumentException("User not found");

        var passports = await Context.Passports
            .Where(p => isoList.Contains(p.IsoShortCode))
            .ToListAsync();

        var alreadyLinked = user.Passports.Select(p => p.IsoShortCode).ToHashSet();
        foreach (var p in passports)
        {
            if (!alreadyLinked.Contains(p.IsoShortCode))
                user.Passports.Add(p);
        }

        await Context.SaveChangesAsync();
    }

    public async Task LinkCountriesToUserAsync(IEnumerable<string> isos, string username)
    {
        var isoList = isos.ToList();
        var user = await Context.Users
            .Include(u => u.Countries)
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null) throw new ArgumentException("User not found");

        var countries = await Context.Countries
            .Where(c => isoList.Contains(c.IsoShortCode))
            .ToListAsync();

        var alreadyLinked = user.Countries.Select(c => c.IsoShortCode).ToHashSet();
        foreach (var c in countries)
        {
            if (!alreadyLinked.Contains(c.IsoShortCode))
                user.Countries.Add(c);
        }

        await Context.SaveChangesAsync();
    }

    public async Task UnlinkPassportsFromUserAsync(string username, IEnumerable<string> isos)
    {
        var isoSet = isos.ToHashSet();
        var user = await Context.Users
            .Include(u => u.Passports)
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null) return;

        var toRemove = user.Passports.Where(p => isoSet.Contains(p.IsoShortCode)).ToList();
        foreach (var p in toRemove)
            user.Passports.Remove(p);

        await Context.SaveChangesAsync();
    }

    public async Task UnlinkCountriesFromUserAsync(string username, IEnumerable<string> isos)
    {
        var isoSet = isos.ToHashSet();
        var user = await Context.Users
            .Include(u => u.Countries)
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null) return;

        var toRemove = user.Countries.Where(c => isoSet.Contains(c.IsoShortCode)).ToList();
        foreach (var c in toRemove)
            user.Countries.Remove(c);

        await Context.SaveChangesAsync();
    }

    public async Task UpdateUserVisibilityAsync(string username, bool? showPassports, bool? showCountries)
    {
        var user = await Context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null) return;

        if (showPassports.HasValue) user.ShowPassports = showPassports.Value;
        if (showCountries.HasValue) user.ShowCountries = showCountries.Value;

        await Context.SaveChangesAsync();
    }
}