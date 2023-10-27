using MediatR;

namespace UserCacheService.Application.UserInfo.Create;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Domain.UserInfo.UserInfo>
{
    public Task<Domain.UserInfo.UserInfo> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}