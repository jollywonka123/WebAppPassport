using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppPassport.Converters;
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

        var stack = await userService.GetStackAsync(username);
        if (stack == null) return NotFound("No passports linked to this user");

        return Ok(stack.ToStackViewModel());
    }
}
