using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UserCacheService.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    public AuthController()
    {
        
    }
    
    [HttpGet]
    [Route("[action]")]
    public IActionResult Test()
    {
        return Ok(new { message = "ok" });
    }
}