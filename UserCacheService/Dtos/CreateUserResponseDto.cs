namespace UserCacheService.Dtos;

public class CreateUserResponseDto : BaseResponseDto
{
    public int ErrorId { get; set; } = 0;
    
    public UserInfoDto User { get; set; }
}