using MediatR;

namespace UserCacheService.Application.UserInfo.Remove;

public class RemoveUserCommand : IRequest<Domain.UserInfo.UserInfo>
{
    public int Id { get; }

    public RemoveUserCommand(int id)
    {
        Id = id;
    }
}