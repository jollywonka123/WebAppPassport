using Microsoft.AspNetCore.Mvc;
using WebAppPassport.Converters;
using WebAppPassport.Services.RankService;

namespace WebAppPassport.Controllers;

[ApiController]
[Route("rank")]
public class RankController(IRankService rankService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetRank()
    {
        var (passports, countries) = await rankService.GetRankAsync();
        return Ok(FromServiceToViewModelConverter.ToRankViewModel(passports, countries));
    }
}
