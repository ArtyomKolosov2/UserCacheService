using System.Xml.Serialization;

namespace UserCacheService.Console.Dtos;

public class BaseResponseDto
{
    [XmlAttribute]
    public bool Success { get; set; }
}