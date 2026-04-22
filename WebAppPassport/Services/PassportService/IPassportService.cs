using WebAppPassport.Services.ResponseModels;

namespace WebAppPassport.Services.PassportService;

public interface IPassportService
{
    Task<ICollection<PassportListItem>> GetAllPassportsAsync();

    Task<PassportDetail?> GetPassportDetailAsync(string isoShortCode);

    Task<PassportRankInfo?> GetPassportByIsoAsync(string isoShortCode);
}
