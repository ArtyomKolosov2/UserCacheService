using MediatR;

namespace UserCacheService.Application.UserInfo.Remove;

public class RemoveUserCommandHandler : IRequestHandler<RemoveUserCommand, Domain.UserInfo.UserInfo>
{
    public Task<Domain.UserInfo.UserInfo> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}