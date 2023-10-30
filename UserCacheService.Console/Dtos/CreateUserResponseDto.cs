using System.Xml.Serialization;

namespace UserCacheService.Console.Dtos;

[XmlRoot("Response")]
public class CreateUserResponseDto : BaseResponseDto
{
    [XmlAttribute]
    public int ErrorId { get; set; } = 0;
    
    [XmlElement("user")]
    public UserInfoDto User { get; set; }
}