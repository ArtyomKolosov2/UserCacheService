using System.Xml.Serialization;

namespace UserCacheService.Dtos;

public class BaseResponseDto
{
    [XmlAttribute]
    public bool Success { get; set; }
}