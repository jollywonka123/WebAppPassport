using Microsoft.AspNetCore.Mvc;
using WebAppPassport.Services.ResponseModels;
using WebAppPassport.Services.UserService;

namespace WebAppPassport.Controllers;

[ApiController]
[Route("user")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var success = await userService.RegisterAsync(request.Username, request.Password);
        if (!success) return Conflict("Username already taken");
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await userService.LoginAsync(request.Username, request.Password);
        if (result == null) return Unauthorized("Invalid credentials");
        return Ok(result);
    }
}
