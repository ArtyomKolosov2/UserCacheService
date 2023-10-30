using System.Xml.Serialization;

namespace UserCacheService.ConsoleApplication.Dtos;

public class BaseResponseDto
{
    [XmlAttribute]
    public bool Success { get; set; }
}