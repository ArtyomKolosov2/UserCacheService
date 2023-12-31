﻿using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserCacheService.Application.UserInfo.Get;
using UserCacheService.Dtos;

namespace UserCacheService.Controllers;

[AllowAnonymous]
[Route("[controller]")]
public class PublicController : Controller
{
    private readonly IMediator _mediator;

    public PublicController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Route("UserInfo")]
    public async Task<IActionResult> GetUserInfo([FromQuery] int id, CancellationToken cancellationToken)
    {
        var userInfo = await _mediator.Send(new GetUserCommand(id), cancellationToken);
        return View(userInfo.Adapt<UserInfoDto>());
    }
}