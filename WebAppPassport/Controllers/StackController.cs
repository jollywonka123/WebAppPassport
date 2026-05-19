using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppPassport.Controllers.Models;
using WebAppPassport.Converters;
using WebAppPassport.Services.UserService;

namespace WebAppPassport.Controllers;

[ApiController]
[Route("stack")]
[Authorize]
[Produces("application/json")]
public class StackController(IUserService userService) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Получить стек доступных направлений текущего пользователя")]
    [EndpointDescription("Возвращает агрегированный список стран, доступных владельцу всех его паспортов, с разбивкой по типу въезда: безвизовый, виза по прибытии, электронное разрешение и т.д. Требуется JWT-токен.")]
    [ProducesResponseType<Dictionary<string, List<CountrySummaryViewModel>>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Dictionary<string, List<CountrySummaryViewModel>>>> GetStack()
    {
        var username = User.FindFirstValue(ClaimTypes.Name);
        if (username == null) return Unauthorized();

        var stack = await userService.GetStackAsync(username);
        if (stack == null) return NotFound("No passports linked to this user");

        return Ok(stack.ToStackViewModel());
    }
}
