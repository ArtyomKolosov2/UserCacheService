namespace UserCacheService.Dtos;

public class ErrorResponseDto
{
    public int ErrorId { get; set; }
    
    public bool Success { get; set; }
    
    public string ErrorMessage { get; set; }
}