using WebAppPassport.Common;
using WebAppPassport.Services.Models;

namespace WebAppPassport.Services.UserService;

public interface IUserService
{
    Task<bool> RegisterAsync(string username, string password);

    /// <returns>JWT token string, or null if credentials are invalid.</returns>
    Task<string?> LoginAsync(string username, string password);

    Task<Dictionary<VisaType, List<Country>>?> GetStackAsync(string username);

    Task AddPassportsAsync(string username, IEnumerable<string> isos);

    Task AddCountriesAsync(string username, IEnumerable<string> isos);

    Task RemovePassportsAsync(string username, IEnumerable<string> isos);

    Task RemoveCountriesAsync(string username, IEnumerable<string> isos);

    Task<User?> GetPublicProfileAsync(string username);

    Task SetPassportsVisibilityAsync(string username, bool show);

    Task SetCountriesVisibilityAsync(string username, bool show);
}
