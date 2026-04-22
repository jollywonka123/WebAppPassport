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
}
