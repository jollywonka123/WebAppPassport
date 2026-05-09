using Microsoft.AspNetCore.Mvc;
using WebAppPassport.Converters;
using WebAppPassport.Services.PassportService;

namespace WebAppPassport.Controllers;

[ApiController]
public class CompareController(IPassportService passportService) : ControllerBase
{
    [HttpGet("/compare")]
    public async Task<IActionResult> Compare([FromQuery] string isos)
    {
        var isoList = isos.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (isoList.Length == 0) return BadRequest("No ISO codes provided");

        var passports = await passportService.GetPassportsByIsosAsync(isoList);
        return Ok(passports.Select(p => p.ToDetailViewModel()).ToList());
    }
}
