using UserCacheService.Domain.UserInfo;

namespace UserCacheService.Application.Dtos;

public class UserInfoDto
{
    public int Id { get; set; } 
    
    public string Name { get; set; }
    
    public string Status { get; set; }
}