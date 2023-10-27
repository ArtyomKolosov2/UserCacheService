namespace UserCacheService.Application.Dtos;

public class CreateUserResponseDto
{
    public bool Success { get; set; } = true;

    public int ErrorId { get; set; } = 0;
    
    public UserInfoDto User { get; set; }
}