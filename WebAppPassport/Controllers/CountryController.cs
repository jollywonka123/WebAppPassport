using Microsoft.AspNetCore.Mvc;
using WebAppPassport.Services.CountryService;

namespace WebAppPassport.Controllers;

[ApiController]
[Route("country")]
public class CountryController(ICountryService countryService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await countryService.GetAllCountriesAsync();
        return Ok(result);
    }

    [HttpGet("{isoShortCode}")]
    public async Task<IActionResult> GetDetail(string isoShortCode)
    {
        var result = await countryService.GetCountryDetailAsync(isoShortCode);
        if (result == null) return NotFound();
        return Ok(result);
    }
}
