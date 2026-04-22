using WebAppPassport.Converters;
using WebAppPassport.DataBase.Repositories;
using WebAppPassport.Services.ResponseModels;

namespace WebAppPassport.Services.RankService;

public class RankService(IRepository repository) : IRankService
{
    public async Task<RankResponse> GetRankAsync()
    {
        var efPassports = await repository.GetAllPassportsOrderedByRankAsync();
        var efCountries = await repository.GetAllCountriesWithPassportAsync();

        var passports = efPassports.ToServiceEntity();
        var countries = efCountries.ToServiceEntity();

        var passportItems = passports.Select(p => new RankPassportItem
        {
            Name = p.Name,
            IsoShortCode = p.IsoShortCode,
            MobilityScore = p.MobilityScore,
            WorldRank = p.WorldRank
        }).ToList();

        var countryItems = countries.Select(c => new RankCountryItem
        {
            Name = c.Name,
            IsoShortCode = c.IsoShortCode,
            MobilityScore = c.Passport?.MobilityScore ?? 0,
            WorldRank = c.Passport?.WorldRank ?? 0
        }).ToList();

        return new RankResponse
        {
            Passports = passportItems,
            Countries = countryItems
        };
    }
}
