using Microsoft.AspNetCore.Mvc;
using NewsPortal.Auth;

namespace NewsPortal.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AccountService _accountService;

    public AuthController(AccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var token = await _accountService.Login(request.UserName, request.Password);
            return Ok(new { Token = token });
        }
        catch (Exception ex)
        {
            return Unauthorized(new { Error = "Invalid credentials" });
        }
    }

}

public class LoginRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
}
