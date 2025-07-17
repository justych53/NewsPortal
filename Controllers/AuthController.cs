using Microsoft.AspNetCore.Mvc;
using NewsPortal.Auth;

namespace NewsPortal.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(AccountService accountService): ControllerBase
{
    [HttpPost("register")]

    public IActionResult Register([FromBody]RegisterUserRequest request)
    {
        accountService.Register(request.UserName, request.FirstName, request.Password);
        return NoContent();
    }

    [HttpPost("login")]
    public IActionResult Login(string userName, string password)
    {
        var token = accountService.Login(userName, password);
        return Ok(token);
    }
}
