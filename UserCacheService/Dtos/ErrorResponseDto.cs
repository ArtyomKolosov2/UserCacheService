namespace UserCacheService.Dtos;

public class ErrorResponseDto : BaseResponseDto
{
    public int ErrorId { get; set; }
    
    public string ErrorMessage { get; set; }
}