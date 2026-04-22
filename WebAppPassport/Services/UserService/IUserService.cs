using WebAppPassport.Services.ResponseModels;

namespace WebAppPassport.Services.UserService;

public interface IUserService
{
    Task<bool> RegisterAsync(string username, string password);

    Task<LoginResponse?> LoginAsync(string username, string password);

    Task<Dictionary<string, List<CountrySummary>>?> GetStackAsync(string username);
}
