using Microsoft.AspNetCore.Mvc;
using WebAppPassport.Services.PassportService;

namespace WebAppPassport.Controllers;

[ApiController]
public class PassportController(IPassportService passportService) : ControllerBase
{
    [HttpGet("/")]
    public async Task<IActionResult> GetAll()
    {
        var result = await passportService.GetAllPassportsAsync();
        return Ok(result);
    }

    [HttpGet("/passport/{isoShortCode}")]
    public async Task<IActionResult> GetDetail(string isoShortCode)
    {
        var result = await passportService.GetPassportDetailAsync(isoShortCode);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet("/passport")]
    public async Task<IActionResult> GetByIso([FromQuery] string iso)
    {
        var result = await passportService.GetPassportByIsoAsync(iso);
        if (result == null) return NotFound();
        return Ok(result);
    }
}
