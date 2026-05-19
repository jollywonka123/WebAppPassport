using Microsoft.AspNetCore.Mvc;
using WebAppPassport.Controllers.Models;
using WebAppPassport.Converters;
using WebAppPassport.Services.CountryService;

namespace WebAppPassport.Controllers;

[ApiController]
[Route("country")]
[Produces("application/json")]
public class CountryController(ICountryService countryService) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Получить список всех стран")]
    [EndpointDescription("Возвращает список всех стран с базовой информацией: название, ISO-код и ссылка на детальную страницу.")]
    [ProducesResponseType<List<CountryListItemViewModel>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var countries = await countryService.GetAllCountriesAsync();
        return Ok(countries.Select(c => c.ToListItemViewModel()).ToList());
    }

    [HttpGet("{isoShortCode}")]
    [EndpointSummary("Получить детальную информацию о стране")]
    [EndpointDescription("Возвращает подробные сведения о стране по ISO-коду: население, площадь, возможность двойного гражданства и список паспортных направлений с разбивкой по типу визы.")]
    [ProducesResponseType<CountryDetailViewModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDetail(string isoShortCode)
    {
        var country = await countryService.GetCountryDetailAsync(isoShortCode);
        if (country == null) return NotFound();
        return Ok(country.ToDetailViewModel());
    }
}
