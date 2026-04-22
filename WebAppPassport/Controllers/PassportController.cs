using Microsoft.AspNetCore.Mvc;
using WebAppPassport.Converters;
using WebAppPassport.Services.PassportService;

namespace WebAppPassport.Controllers;

[ApiController]
public class PassportController(IPassportService passportService) : ControllerBase
{
    [HttpGet("/")]
    public async Task<IActionResult> GetAll()
    {
        var passports = await passportService.GetAllPassportsAsync();
        return Ok(passports.Select(p => p.ToListItemViewModel()).ToList());
    }

    [HttpGet("/passport/{isoShortCode}")]
    public async Task<IActionResult> GetDetail(string isoShortCode)
    {
        var passport = await passportService.GetPassportDetailAsync(isoShortCode);
        if (passport == null) return NotFound();
        return Ok(passport.ToDetailViewModel());
    }

    [HttpGet("/passport")]
    public async Task<IActionResult> GetByIso([FromQuery] string iso)
    {
        var passport = await passportService.GetPassportByIsoAsync(iso);
        if (passport == null) return NotFound();
        return Ok(passport.ToRankInfoViewModel());
    }
}
