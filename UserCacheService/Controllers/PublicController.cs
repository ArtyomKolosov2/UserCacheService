using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserCacheService.Application.UserInfo.Get;
using UserCacheService.Dtos;

namespace UserCacheService.Controllers;

[Route("[controller]")]
public class PublicController : Controller
{
    private readonly IMediator _mediator;

    public PublicController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Route("UserInfo")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUserInfo([FromQuery] int id)
    {
        var userInfo = await _mediator.Send(new GetUserCommand(id));
        return View(userInfo.Adapt<UserInfoDto>());
    }
}