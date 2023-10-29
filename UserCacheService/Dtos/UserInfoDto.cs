using System.Xml.Serialization;

namespace UserCacheService.Dtos;

public class UserInfoDto
{
    [XmlAttribute]
    public int Id { get; set; } 
    
    [XmlAttribute]
    public string Name { get; set; }
    
    [XmlElement]
    public string Status { get; set; }
}