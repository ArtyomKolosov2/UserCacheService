using MediatR;
using UserCacheService.Domain.UserInfo.Cache;

namespace UserCacheService.Application.UserInfo.Cache.Update;

public class UpdateUserCacheCommandHandler : IRequestHandler<UpdateUserCacheCommand>
{
    private readonly IUserInfoCache _userInfoCache;

    public UpdateUserCacheCommandHandler(IUserInfoCache userInfoCache)
    {
        _userInfoCache = userInfoCache;
    }
    
    public Task Handle(UpdateUserCacheCommand request, CancellationToken cancellationToken) 
        => _userInfoCache.RefreshCache(cancellationToken);
}