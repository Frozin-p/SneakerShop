using AuthService.Abstractions;
using AuthService.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserDto createUserDto)
    {
        var user = new User
        {
            Username = createUserDto.Username,
            Email = createUserDto.Email,
            PasswordHash = createUserDto.PasswordHash,
            CreatedAt = createUserDto.CreatedAt
        };

        try
        {
            await _userService.RegisterUser(user);
            return Ok(user);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] User user)
    {
        var result = await _userService.AuthenticateUser(user.Username, user.PasswordHash);
        return result != null ? Ok(result) : Unauthorized();
    }
}
