using Microsoft.AspNetCore.Mvc;
using WebAppPassport.Controllers.Models;
using WebAppPassport.Converters;
using WebAppPassport.Services.RankService;

namespace WebAppPassport.Controllers;

[ApiController]
[Route("rank")]
[Produces("application/json")]
public class RankController(IRankService rankService) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Получить глобальный рейтинг паспортов и стран")]
    [EndpointDescription("Возвращает мировой рейтинг паспортов и стран, отсортированный по индексу мобильности. Содержит позицию каждого паспорта/страны в мировом рейтинге и количество доступных направлений.")]
    [ProducesResponseType<RankViewModel>(StatusCodes.Status200OK)]
    public async Task<ActionResult<RankViewModel>> GetRank()
    {
        var (passports, countries) = await rankService.GetRankAsync();
        return Ok(FromServiceToViewModelConverter.ToRankViewModel(passports, countries));
    }
}
