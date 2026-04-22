using WebAppPassport.Services.Models;

namespace WebAppPassport.Services.PassportService;

public interface IPassportService
{
    Task<ICollection<Passport>> GetAllPassportsAsync();

    Task<Passport?> GetPassportDetailAsync(string isoShortCode);

    Task<Passport?> GetPassportByIsoAsync(string isoShortCode);
}
