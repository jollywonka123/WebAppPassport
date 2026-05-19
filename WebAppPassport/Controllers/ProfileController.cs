using Microsoft.AspNetCore.Mvc;
using WebAppPassport.Controllers.Models;
using WebAppPassport.Converters;
using WebAppPassport.Services.UserService;

namespace WebAppPassport.Controllers;

[ApiController]
[Produces("application/json")]
public class ProfileController(IUserService userService) : ControllerBase
{
    [HttpGet("/u/{username}")]
    [EndpointSummary("Получить публичный профиль пользователя")]
    [EndpointDescription("Возвращает публичную информацию профиля пользователя: имя, список паспортов и посещённых стран (если пользователь разрешил их показ). Если видимость отключена — возвращается только количество элементов.")]
    [ProducesResponseType<ProfileViewModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProfile(string username)
    {
        var user = await userService.GetPublicProfileAsync(username);
        if (user == null) return NotFound();
        return Ok(user.ToProfileViewModel());
    }
}
