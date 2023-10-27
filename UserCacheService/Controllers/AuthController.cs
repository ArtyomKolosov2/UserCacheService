using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserCacheService.Application.Dtos;

namespace UserCacheService.Controllers;

[Authorize]
[ApiController]
[Route("[controller]/[action]")]
public class AuthController : ControllerBase
{
    public AuthController()
    {
        
    }
    
    [HttpPost]
    [Consumes("application/xml")]
    [Produces("application/xml")]
    public Task<ActionResult<CreateUserResponseDto>> CreateUser([FromBody] CreateUserRequestDto createUserRequestDto)
    {
        return new Task<ActionResult<CreateUserResponseDto>>(null);
    }

    [HttpPost]
    [Consumes("application/x-www-form-urlencoded")]
    [Produces("application/json")]
    public Task<ActionResult<UserInfoDto>> SetStatus([FromForm] int id, [FromForm] string newStatus)
    {
        return new Task<ActionResult<UserInfoDto>>(null);
    }

    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    public Task<ActionResult<RemoveUserResponseDto>> RemoveUser([FromBody] RemoveUserRequestDto removeUserRequestDto)
    {
        return new Task<ActionResult<RemoveUserResponseDto>>(null);
    }
}