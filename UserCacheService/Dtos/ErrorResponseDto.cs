using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace UserCacheService.Dtos;

[XmlRoot("Response")]
public class ErrorResponseDto : BaseResponseDto
{
    [XmlAttribute]
    public int ErrorId { get; set; }
    
    [XmlElement("ErrorMsg")]
    [JsonPropertyName("Msg")]
    public string ErrorMessage { get; set; }
}