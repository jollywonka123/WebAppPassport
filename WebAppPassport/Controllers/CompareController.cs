using Microsoft.AspNetCore.Mvc;
using WebAppPassport.Controllers.Models;
using WebAppPassport.Converters;
using WebAppPassport.Services.PassportService;

namespace WebAppPassport.Controllers;

[ApiController]
[Produces("application/json")]
public class CompareController(IPassportService passportService) : ControllerBase
{
    [HttpGet("/compare")]
    [EndpointSummary("Сравнить несколько паспортов")]
    [EndpointDescription("Возвращает детальную информацию для нескольких паспортов одновременно. Параметр `isos` принимает список ISO-кодов через запятую (например: `RU,DE,US`). Удобно для визуального сравнения условий въезда разных стран.")]
    [ProducesResponseType<List<PassportDetailViewModel>>(StatusCodes.Status200OK)]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<PassportDetailViewModel>>> Compare([FromQuery] string isos)
    {
        var isoList = isos.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (isoList.Length == 0) return BadRequest("No ISO codes provided");

        var passports = await passportService.GetPassportsByIsosAsync(isoList);
        return Ok(passports.Select(p => p.ToDetailViewModel()).ToList());
    }
}
