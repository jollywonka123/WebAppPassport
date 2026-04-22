using WebAppPassport.Converters;
using WebAppPassport.DataBase.Repositories;
using WebAppPassport.Services.Models;

namespace WebAppPassport.Services.RankService;

public class RankService(IRepository repository) : IRankService
{
    public async Task<(ICollection<Passport> Passports, ICollection<Country> Countries)> GetRankAsync()
    {
        var efPassports = await repository.GetAllPassportsOrderedByRankAsync();
        var efCountries = await repository.GetAllCountriesWithPassportAsync();
        return (efPassports.ToServiceEntity(), efCountries.ToServiceEntity());
    }
}
