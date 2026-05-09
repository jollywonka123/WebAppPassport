using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppPassport.Controllers.Models;
using WebAppPassport.Converters;
using WebAppPassport.Services.UserService;

namespace WebAppPassport.Controllers;

[ApiController]
[Route("user")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var (username, password) = request.ToCredentials();
        var success = await userService.RegisterAsync(username, password);
        if (!success) return Conflict("Username already taken");
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var (username, password) = request.ToCredentials();
        var token = await userService.LoginAsync(username, password);
        if (token == null) return Unauthorized("Invalid credentials");
        return Ok(new TokenViewModel { Token = token });
    }

    [HttpPost("addPassport")]
    [Authorize]
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
    public async Task<IActionResult> SetPassportsVisibility([FromBody] VisibilityRequest request)
    {
        var username = User.FindFirstValue(ClaimTypes.Name);
        if (username == null) return Unauthorized();

        await userService.SetPassportsVisibilityAsync(username, request.Show);
        return Ok();
    }

    [HttpPatch("visibility/countries")]
    [Authorize]
    public async Task<IActionResult> SetCountriesVisibility([FromBody] VisibilityRequest request)
    {
        var username = User.FindFirstValue(ClaimTypes.Name);
        if (username == null) return Unauthorized();

        await userService.SetCountriesVisibilityAsync(username, request.Show);
        return Ok();
    }
}
