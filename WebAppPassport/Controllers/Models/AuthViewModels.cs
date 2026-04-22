namespace WebAppPassport.Controllers.Models;

public class RegisterRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}

public class LoginRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}

public class TokenViewModel
{
    public required string Token { get; set; }
}
