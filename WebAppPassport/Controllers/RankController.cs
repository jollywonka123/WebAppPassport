using Microsoft.AspNetCore.Mvc;
using WebAppPassport.Services.RankService;

namespace WebAppPassport.Controllers;

[ApiController]
[Route("rank")]
public class RankController(IRankService rankService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetRank()
    {
        var result = await rankService.GetRankAsync();
        return Ok(result);
    }
}
