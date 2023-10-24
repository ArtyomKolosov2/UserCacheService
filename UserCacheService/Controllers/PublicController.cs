using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UserCacheService.Controllers;

[AllowAnonymous]
[ApiController]
[Route("[controller]")]
public class PublicController : ControllerBase
{
    public PublicController()
    {
        
    }
}