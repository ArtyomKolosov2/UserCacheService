namespace UserCacheService.Domain.UserInfo;

public class UserInfo
{
    public UserInfo()
    {
        
    }
    
    public UserInfo(string name)
    {
        Name = name;
    }
    
    // There is no enforced rule to make Id always positive int since there is no such requirement.
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public UserInfoStatus Status { get; set; }  
}