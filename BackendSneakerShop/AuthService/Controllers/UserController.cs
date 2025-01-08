using AuthService.Abstractions;
using AuthService.Contracts;
using AuthService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly RabbitMqPublisher _rabbitMqPublisher;

    public UserController(IUserService userService, RabbitMqPublisher rabbitMqPublisher)
    {
        _userService = userService;
        _rabbitMqPublisher = rabbitMqPublisher;
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

            _rabbitMqPublisher.Publish(new
            {
                UserId = user.Id,
            });

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
