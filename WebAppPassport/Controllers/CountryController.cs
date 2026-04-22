using Microsoft.AspNetCore.Mvc;
using WebAppPassport.Converters;
using WebAppPassport.Services.CountryService;

namespace WebAppPassport.Controllers;

[ApiController]
[Route("country")]
public class CountryController(ICountryService countryService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var countries = await countryService.GetAllCountriesAsync();
        return Ok(countries.Select(c => c.ToListItemViewModel()).ToList());
    }

    [HttpGet("{isoShortCode}")]
    public async Task<IActionResult> GetDetail(string isoShortCode)
    {
        var country = await countryService.GetCountryDetailAsync(isoShortCode);
        if (country == null) return NotFound();
        return Ok(country.ToDetailViewModel());
    }
}
