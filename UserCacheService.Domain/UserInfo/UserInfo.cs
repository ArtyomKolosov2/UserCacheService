namespace UserCacheService.Domain.UserInfo;

public class UserInfo
{
    public UserInfo(string name)
    {
        Name = name;
    }
    
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public UserInfoStatus Status { get; set; }  
}