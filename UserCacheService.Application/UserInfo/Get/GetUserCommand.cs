using MediatR;

namespace UserCacheService.Application.UserInfo.Get;

public class GetUserCommand : IRequest<Domain.UserInfo.UserInfo>
{
    public int Id { get; }

    public GetUserCommand(int id)
    {
        Id = id;
    }
}