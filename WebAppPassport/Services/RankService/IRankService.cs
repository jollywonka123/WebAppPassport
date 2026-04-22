using WebAppPassport.Services.Models;

namespace WebAppPassport.Services.RankService;

public interface IRankService
{
    Task<(ICollection<Passport> Passports, ICollection<Country> Countries)> GetRankAsync();
}
