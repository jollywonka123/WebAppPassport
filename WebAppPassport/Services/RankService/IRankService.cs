using WebAppPassport.Services.ResponseModels;

namespace WebAppPassport.Services.RankService;

public interface IRankService
{
    Task<RankResponse> GetRankAsync();
}
