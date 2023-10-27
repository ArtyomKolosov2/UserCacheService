using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserCacheService.Application.Dtos;

namespace UserCacheService.Controllers;

[AllowAnonymous]
[Route("[controller]")]
public class PublicController : Controller
{
    public PublicController()
    {
        
    }
    
    [Route("[action]")]
    public IActionResult GetUserInfo([FromQuery] int id)
    {
        return View(new UserInfoDto
        {
            Name = "Test",
            Status = "Test"
        });
    }
}