using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebAppPassport.Common;
using WebAppPassport.Converters;
using WebAppPassport.DataBase.Models;
using WebAppPassport.DataBase.Repositories;
using ServiceModels = WebAppPassport.Services.Models;

namespace WebAppPassport.Services.UserService;

public class UserService(IRepository repository) : IUserService
{
    public async Task<bool> RegisterAsync(string username, string password)
    {
        var existing = await repository.GetUserByUsernameAsync(username);
        if (existing != null) return false;

        var serviceUser = new ServiceModels.User
        {
            Username = username,
            HashedPassword = BCrypt.Net.BCrypt.HashPassword(password)
        };

        await repository.AddUserAsync(serviceUser.ToEfEntity());
        return true;
    }

    public async Task<string?> LoginAsync(string username, string password)
    {
        var efUser = await repository.GetUserByUsernameAsync(username);
        if (efUser == null || !BCrypt.Net.BCrypt.Verify(password, efUser.HashedPassword))
            return null;

        return GenerateJwt(efUser);
    }

    public async Task<Dictionary<VisaType, List<ServiceModels.Country>>?> GetStackAsync(string username)
    {
        var efUser = await repository.GetUserWithPassportsAndDestinationsAsync(username);
        if (efUser == null) return null;

        var user = efUser.ToServiceEntity();
        if (user.Passports == null || !user.Passports.Any()) return null;

        var buckets = new Dictionary<VisaType, HashSet<string>>
        {
            [VisaType.VisaFree] = new(),
            [VisaType.VisaOnArrival] = new(),
            [VisaType.EVisa] = new(),
            [VisaType.RequiredVisa] = new()
        };

        var countryLookup = user.Passports
            .SelectMany(p => p.Destinations ?? [])
            .GroupBy(d => d.Country.IsoShortCode)
            .ToDictionary(g => g.Key, g => g.First().Country);

        foreach (var passport in user.Passports)
            foreach (var dest in passport.Destinations ?? [])
                buckets[dest.VisaType].Add(dest.Country.IsoShortCode);

        ServiceModels.Country Resolve(string iso) =>
            countryLookup.GetValueOrDefault(iso)
            ?? new ServiceModels.Country { Name = iso, IsoShortCode = iso };

        var free = buckets[VisaType.VisaFree].ToHashSet();
        var onArrival = buckets[VisaType.VisaOnArrival].Except(free).ToHashSet();
        var evisa = buckets[VisaType.EVisa].Except(free).Except(onArrival).ToHashSet();
        var allAccounted = free.Concat(onArrival).Concat(evisa).ToHashSet();
        var required = buckets[VisaType.RequiredVisa].Except(allAccounted).ToHashSet();

        return new Dictionary<VisaType, List<ServiceModels.Country>>
        {
            [VisaType.VisaFree] = free.Select(Resolve).ToList(),
            [VisaType.VisaOnArrival] = onArrival.Select(Resolve).ToList(),
            [VisaType.EVisa] = evisa.Select(Resolve).ToList(),
            [VisaType.RequiredVisa] = required.Select(Resolve).ToList()
        };
    }

    // JWT generation accesses the persisted EF User Id — kept as internal implementation detail
    private static string GenerateJwt(User user)
    {
        var secret = Environment.GetEnvironmentVariable("JWT_SECRET")
            ?? throw new InvalidOperationException("JWT_SECRET is not set");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims:
            [
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            ],
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
