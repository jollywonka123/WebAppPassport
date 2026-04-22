using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebAppPassport.Common;
using WebAppPassport.Converters;
using WebAppPassport.DataBase.Models;
using WebAppPassport.DataBase.Repositories;
using WebAppPassport.Services.ResponseModels;
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

    public async Task<LoginResponse?> LoginAsync(string username, string password)
    {
        var efUser = await repository.GetUserByUsernameAsync(username);
        if (efUser == null) return null;
        if (!BCrypt.Net.BCrypt.Verify(password, efUser.HashedPassword)) return null;

        return new LoginResponse { Token = GenerateJwt(efUser) };
    }

    public async Task<Dictionary<string, List<CountrySummary>>?> GetStackAsync(string username)
    {
        var efUser = await repository.GetUserWithPassportsAndDestinationsAsync(username);
        if (efUser == null) return null;

        var user = efUser.ToServiceEntity();
        if (user.Passports == null || !user.Passports.Any()) return null;

        var visaFree = new HashSet<string>();
        var visaOnArrival = new HashSet<string>();
        var eVisa = new HashSet<string>();
        var required = new HashSet<string>();

        var buckets = new Dictionary<VisaType, HashSet<string>>
        {
            [VisaType.VisaFree] = visaFree,
            [VisaType.VisaOnArrival] = visaOnArrival,
            [VisaType.EVisa] = eVisa,
            [VisaType.RequiredVisa] = required
        };

        foreach (var passport in user.Passports)
        {
            foreach (var dest in passport.Destinations ?? [])
                buckets[dest.VisaType].Add(dest.Country.IsoShortCode);
        }

        var isoToName = user.Passports
            .SelectMany(p => p.Destinations ?? [])
            .GroupBy(d => d.Country.IsoShortCode)
            .ToDictionary(g => g.Key, g => g.First().Country.Name);

        CountrySummary ToSummary(string iso) => new()
        {
            IsoShortCode = iso,
            Name = isoToName.GetValueOrDefault(iso, iso)
        };

        var stackedVisaFree = visaFree.ToHashSet();
        var stackedVisaOnArrival = visaOnArrival.Except(stackedVisaFree).ToHashSet();
        var stackedEVisa = eVisa.Except(stackedVisaFree).Except(stackedVisaOnArrival).ToHashSet();
        var allAccounted = stackedVisaFree.Concat(stackedVisaOnArrival).Concat(stackedEVisa).ToHashSet();
        var stackedRequired = required.Except(allAccounted).ToHashSet();

        return new Dictionary<string, List<CountrySummary>>
        {
            [VisaTypeLabels.VisaFree] = stackedVisaFree.Select(ToSummary).ToList(),
            [VisaTypeLabels.VisaOnArrival] = stackedVisaOnArrival.Select(ToSummary).ToList(),
            [VisaTypeLabels.EVisa] = stackedEVisa.Select(ToSummary).ToList(),
            [VisaTypeLabels.VisaRequired] = stackedRequired.Select(ToSummary).ToList()
        };
    }

    // JWT generation uses the EF User entity to access the persisted Id
    private static string GenerateJwt(User user)
    {
        var secret = Environment.GetEnvironmentVariable("JWT_SECRET")
            ?? throw new InvalidOperationException("JWT_SECRET is not set");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
