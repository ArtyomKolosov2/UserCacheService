using System.Xml.Serialization;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserCacheService.Application.UserInfo.Create;
using UserCacheService.Application.UserInfo.Remove;
using UserCacheService.Application.UserInfo.Set;
using UserCacheService.Domain.UserInfo;
using UserCacheService.Dtos;

namespace UserCacheService.Controllers;

[Authorize]
[ApiController]
[Route("[controller]/[action]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    [Consumes("application/xml")]
    [Produces("application/xml")]
    public async Task<ActionResult<CreateUserResponseDto>> CreateUser([FromBody] CreateUserRequestDto createUserRequestDto, CancellationToken cancellationToken)
    {
        var userInfoCreationResult = await _mediator.Send(new CreateUserCommand(createUserRequestDto.User.Adapt<UserInfo>()), cancellationToken);
        return Ok(new CreateUserResponseDto
        {
            Success = true,
            User = userInfoCreationResult.Adapt<UserInfoDto>()
        });
    }

    [HttpPost]
    [Consumes("application/x-www-form-urlencoded")]
    [Produces("application/json")]
    public async Task<ActionResult<UserInfoDto>> SetStatus([FromForm] int id, [FromForm] string newStatus, CancellationToken cancellationToken)
    {
        var userInfo = await _mediator.Send(new SetUserStatusCommand(id, newStatus), cancellationToken);
        return Ok(userInfo.Adapt<UserInfoDto>());
    }

    [HttpPost]
    [Consumes("application/json")]
    [Produces("application/json")]
    public async Task<ActionResult<RemoveUserResponseDto>> RemoveUser([FromBody] RemoveUserRequestDto removeUserRequestDto, CancellationToken cancellationToken)
    {
        var userInfo = await _mediator.Send(new RemoveUserCommand(removeUserRequestDto.RemoveUser.Id), cancellationToken);
        return Ok(new RemoveUserResponseDto
        {
            User = userInfo.Adapt<UserInfoDto>(),
            Success = true,
            Message = "User was removed"
        });
    }
}