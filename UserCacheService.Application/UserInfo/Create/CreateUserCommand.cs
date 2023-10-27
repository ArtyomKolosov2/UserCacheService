using MediatR;

namespace UserCacheService.Application.UserInfo.Create;

public class CreateUserCommand : IRequest<Domain.UserInfo.UserInfo>
{
    public Domain.UserInfo.UserInfo UserInfo { get; }

    public CreateUserCommand(Domain.UserInfo.UserInfo userInfo)
    {
        UserInfo = userInfo;
    }
}