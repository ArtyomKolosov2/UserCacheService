using MediatR;

namespace UserCacheService.Application.UserInfo.Get;

public class GetUserCommandHandler : IRequestHandler<GetUserCommand, Domain.UserInfo.UserInfo>
{
    public Task<Domain.UserInfo.UserInfo> Handle(GetUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}