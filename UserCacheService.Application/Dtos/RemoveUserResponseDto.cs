namespace UserCacheService.Application.Dtos;

public class RemoveUserResponseDto
{
    public bool Success { get; set; }
    
    public string Message { get; set; }
    
    public UserInfoDto User { get; set; }
}