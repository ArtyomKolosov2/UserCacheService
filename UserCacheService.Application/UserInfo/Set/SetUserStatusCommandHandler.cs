using MediatR;

namespace UserCacheService.Application.UserInfo.Set;

public class SetUserStatusCommandHandler : IRequestHandler<SetUserStatusCommand, Domain.UserInfo.UserInfo>
{
    public Task<Domain.UserInfo.UserInfo> Handle(SetUserStatusCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}