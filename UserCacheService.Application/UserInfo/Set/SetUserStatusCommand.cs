using MediatR;

namespace UserCacheService.Application.UserInfo.Set;

public class SetUserStatusCommand : IRequest<Domain.UserInfo.UserInfo>
{
    public int Id { get; }
    
    public string Status { get; }
    
    public SetUserStatusCommand(int id, string status)
    {
        Id = id;
        Status = status;
    }
}