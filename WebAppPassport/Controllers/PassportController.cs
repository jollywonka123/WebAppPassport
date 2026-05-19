using Microsoft.AspNetCore.Mvc;
using WebAppPassport.Controllers.Models;
using WebAppPassport.Converters;
using WebAppPassport.Services.PassportService;

namespace WebAppPassport.Controllers;

[ApiController]
[Produces("application/json")]
public class PassportController(IPassportService passportService) : ControllerBase
{
    [HttpGet("/")]
    [EndpointSummary("Получить список всех паспортов")]
    [EndpointDescription("Возвращает список всех паспортов с базовой информацией: название страны, ISO-код и ссылка на детальную страницу.")]
    [ProducesResponseType<List<PassportListItemViewModel>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<PassportListItemViewModel>>> GetAll()
    {
        var passports = await passportService.GetAllPassportsAsync();
        return Ok(passports.Select(p => p.ToListItemViewModel()).ToList());
    }

    [HttpGet("/passport/{isoShortCode}")]
    [EndpointSummary("Получить детальную информацию о паспорте")]
    [EndpointDescription("Возвращает подробные сведения о паспорте по ISO-коду страны: индекс мобильности, мировой рейтинг, статистику по типам въезда и список всех доступных направлений с разбивкой по визовым категориям.")]
    [ProducesResponseType<PassportDetailViewModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PassportDetailViewModel>> GetDetail(string isoShortCode)
    {
        var passport = await passportService.GetPassportDetailAsync(isoShortCode);
        if (passport == null) return NotFound();
        return Ok(passport.ToDetailViewModel());
    }

    [HttpGet("/passport")]
    [EndpointSummary("Получить рейтинговую информацию о паспорте по ISO-коду")]
    [EndpointDescription("Возвращает краткую рейтинговую информацию о паспорте: позицию в мировом рейтинге, индекс мобильности и ссылку на детальную страницу. ISO-код передаётся как query-параметр.")]
    [ProducesResponseType<PassportRankInfoViewModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PassportRankInfoViewModel>> GetByIso([FromQuery] string iso)
    {
        var passport = await passportService.GetPassportByIsoAsync(iso);
        if (passport == null) return NotFound();
        return Ok(passport.ToRankInfoViewModel());
    }
}
