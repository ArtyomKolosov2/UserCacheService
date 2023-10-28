namespace UserCacheService.Dtos;

public class RemoveUserResponseDto : BaseResponseDto
{
    public int? ErrorId { get; set; } = null;
    
    public string Message { get; set; }
    
    public UserInfoDto User { get; set; }
}