using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppPassport.Controllers.Models;
using WebAppPassport.Converters;
using WebAppPassport.Services.UserService;

namespace WebAppPassport.Controllers;

[ApiController]
[Route("user")]
[Produces("application/json")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost("register")]
    [EndpointSummary("Зарегистрировать нового пользователя")]
    [EndpointDescription("Создаёт новую учётную запись пользователя. Имя пользователя должно быть уникальным. Пароль хранится в зашифрованном виде.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var (username, password) = request.ToCredentials();
        var success = await userService.RegisterAsync(username, password);
        if (!success) return Conflict("Username already taken");
        return Ok();
    }

    [HttpPost("login")]
    [EndpointSummary("Войти в систему")]
    [EndpointDescription("Аутентифицирует пользователя по имени и паролю. В случае успеха возвращает JWT-токен, который необходимо передавать в заголовке `Authorization: Bearer <token>` для защищённых эндпоинтов.")]
    [ProducesResponseType<TokenViewModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var (username, password) = request.ToCredentials();
        var token = await userService.LoginAsync(username, password);
        if (token == null) return Unauthorized("Invalid credentials");
        return Ok(new TokenViewModel { Token = token });
    }

    [HttpPost("addPassport")]
    [Authorize]
    [EndpointSummary("Добавить паспорта в коллекцию пользователя")]
    [EndpointDescription("Привязывает один или несколько паспортов к профилю текущего пользователя. Параметр `isos` — список ISO-кодов через запятую (например: `RU,DE`). Требуется JWT-токен.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddPassport([FromQuery] string isos)
    {
        var username = User.FindFirstValue(ClaimTypes.Name);
        if (username == null) return Unauthorized();

        var isoList = isos.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (isoList.Length == 0) return BadRequest("No ISO codes provided");

        await userService.AddPassportsAsync(username, isoList);
        return Ok();
    }

    [HttpPost("addCountry")]
    [Authorize]
    [EndpointSummary("Добавить посещённые страны в профиль пользователя")]
    [EndpointDescription("Добавляет одну или несколько стран в список посещённых стран текущего пользователя. Параметр `isos` — список ISO-кодов через запятую. Требуется JWT-токен.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddCountry([FromQuery] string isos)
    {
        var username = User.FindFirstValue(ClaimTypes.Name);
        if (username == null) return Unauthorized();

        var isoList = isos.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (isoList.Length == 0) return BadRequest("No ISO codes provided");

        await userService.AddCountriesAsync(username, isoList);
        return Ok();
    }

    [HttpDelete("removePassport")]
    [Authorize]
    [EndpointSummary("Удалить паспорта из коллекции пользователя")]
    [EndpointDescription("Отвязывает один или несколько паспортов от профиля текущего пользователя. Параметр `isos` — список ISO-кодов через запятую. Требуется JWT-токен.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RemovePassport([FromQuery] string isos)
    {
        var username = User.FindFirstValue(ClaimTypes.Name);
        if (username == null) return Unauthorized();

        var isoList = isos.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (isoList.Length == 0) return BadRequest("No ISO codes provided");

        await userService.RemovePassportsAsync(username, isoList);
        return Ok();
    }

    [HttpDelete("removeCountry")]
    [Authorize]
    [EndpointSummary("Удалить страны из профиля пользователя")]
    [EndpointDescription("Убирает одну или несколько стран из списка посещённых стран текущего пользователя. Параметр `isos` — список ISO-кодов через запятую. Требуется JWT-токен.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RemoveCountry([FromQuery] string isos)
    {
        var username = User.FindFirstValue(ClaimTypes.Name);
        if (username == null) return Unauthorized();

        var isoList = isos.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (isoList.Length == 0) return BadRequest("No ISO codes provided");

        await userService.RemoveCountriesAsync(username, isoList);
        return Ok();
    }

    [HttpPatch("visibility/passports")]
    [Authorize]
    [EndpointSummary("Управление видимостью паспортов в публичном профиле")]
    [EndpointDescription("Включает или отключает отображение списка паспортов на публичной странице пользователя. При `show: false` другие пользователи видят только количество паспортов без деталей. Требуется JWT-токен.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> SetPassportsVisibility([FromBody] VisibilityRequest request)
    {
        var username = User.FindFirstValue(ClaimTypes.Name);
        if (username == null) return Unauthorized();

        await userService.SetPassportsVisibilityAsync(username, request.Show);
        return Ok();
    }

    [HttpPatch("visibility/countries")]
    [Authorize]
    [EndpointSummary("Управление видимостью стран в публичном профиле")]
    [EndpointDescription("Включает или отключает отображение списка посещённых стран на публичной странице пользователя. При `show: false` другие пользователи видят только количество стран без деталей. Требуется JWT-токен.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> SetCountriesVisibility([FromBody] VisibilityRequest request)
    {
        var username = User.FindFirstValue(ClaimTypes.Name);
        if (username == null) return Unauthorized();

        await userService.SetCountriesVisibilityAsync(username, request.Show);
        return Ok();
    }
}
