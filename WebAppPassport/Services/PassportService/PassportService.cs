using WebAppPassport.Converters;
using WebAppPassport.DataBase.Repositories;
using WebAppPassport.Services.Models;

namespace WebAppPassport.Services.PassportService;

public class PassportService(IRepository repository) : IPassportService
{
    public async Task<ICollection<Passport>> GetAllPassportsAsync()
    {
        var efPassports = await repository.GetAllPassportsOrderedByRankAsync();
        return efPassports.ToServiceEntity();
    }

    public async Task<Passport?> GetPassportDetailAsync(string isoShortCode)
    {
        var efPassport = await repository.GetPassportWithDestinationsAsync(isoShortCode);
        return efPassport?.ToServiceEntity();
    }

    public async Task<Passport?> GetPassportByIsoAsync(string isoShortCode)
    {
        var efPassports = await repository.GetAllPassportsOrderedByRankAsync();
        return efPassports.ToServiceEntity().FirstOrDefault(p => p.IsoShortCode == isoShortCode);
    }
}
