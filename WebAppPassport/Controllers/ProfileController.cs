using Microsoft.AspNetCore.Mvc;
using WebAppPassport.Converters;
using WebAppPassport.Services.UserService;

namespace WebAppPassport.Controllers;

[ApiController]
public class ProfileController(IUserService userService) : ControllerBase
{
    [HttpGet("/u/{username}")]
    public async Task<IActionResult> GetProfile(string username)
    {
        var user = await userService.GetPublicProfileAsync(username);
        if (user == null) return NotFound();
        return Ok(user.ToProfileViewModel());
    }
}
