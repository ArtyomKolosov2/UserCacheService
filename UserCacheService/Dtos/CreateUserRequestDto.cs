using System.Xml.Serialization;

namespace UserCacheService.Dtos;

[XmlRoot("Request")]
public class CreateUserRequestDto
{
    [XmlElement("user")]
    public UserInfoDto? User { get; set; }
}