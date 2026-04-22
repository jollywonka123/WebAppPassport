using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppPassport.Services.UserService;

namespace WebAppPassport.Controllers;

[ApiController]
[Route("stack")]
[Authorize]
public class StackController(IUserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetStack()
    {
        var username = User.FindFirstValue(ClaimTypes.Name);
        if (username == null) return Unauthorized();

        var result = await userService.GetStackAsync(username);
        if (result == null) return NotFound("No passports linked to this user");
        return Ok(result);
    }
}
