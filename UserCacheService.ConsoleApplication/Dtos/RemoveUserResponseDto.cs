using System.Text.Json.Serialization;

namespace UserCacheService.ConsoleApplication.Dtos;

public class RemoveUserResponseDto : BaseResponseDto
{
    public int? ErrorId { get; set; } = null;
    
    [JsonPropertyName("Msg")]
    public string Message { get; set; }
    
    [JsonPropertyName("user")]
    public UserInfoDto User { get; set; }
}