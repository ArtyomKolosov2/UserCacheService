using MediatR;
using UserCacheService.Domain.UserInfo.Cache;

namespace UserCacheService.Application.UserInfo.Get;

public class GetUserCommandHandler : IRequestHandler<GetUserCommand, Domain.UserInfo.UserInfo>
{
    private readonly IUserInfoCache _userInfoCache;

    public GetUserCommandHandler(IUserInfoCache userInfoCache)
    {
        _userInfoCache = userInfoCache;
    }
    
    public Task<Domain.UserInfo.UserInfo> Handle(GetUserCommand request, CancellationToken cancellationToken)
    {
        return _userInfoCache.GetUserInfoById(request.Id, cancellationToken);
    }
}