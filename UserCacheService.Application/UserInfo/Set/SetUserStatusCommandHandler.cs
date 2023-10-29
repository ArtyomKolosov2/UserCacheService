using MediatR;
using UserCacheService.Domain.Exceptions;
using UserCacheService.Domain.UserInfo;
using UserCacheService.Domain.UserInfo.Repository;

namespace UserCacheService.Application.UserInfo.Set;

public class SetUserStatusCommandHandler : IRequestHandler<SetUserStatusCommand, Domain.UserInfo.UserInfo>
{
    private readonly IUserInfoRepository _userInfoRepository;

    public SetUserStatusCommandHandler(IUserInfoRepository userInfoRepository)
    {
        _userInfoRepository = userInfoRepository;
    }
    
    public async Task<Domain.UserInfo.UserInfo> Handle(SetUserStatusCommand request, CancellationToken cancellationToken)
    {
        var userInfo = await _userInfoRepository.GetById(request.Id, cancellationToken);
        if (userInfo is null)
            throw new UserInfoNotFoundException(request.Id);

        userInfo.Status = TryParseNewStatusOrThrow(request.Status);
        var updatedUserInfo = await _userInfoRepository.Update(userInfo, cancellationToken);
        
        return updatedUserInfo;
    }

    private static UserInfoStatus TryParseNewStatusOrThrow(string newStatus) => newStatus switch
    {
        nameof(UserInfoStatus.New) => UserInfoStatus.New,
        nameof(UserInfoStatus.Active) => UserInfoStatus.Active,
        nameof(UserInfoStatus.Deleted) => UserInfoStatus.Deleted,
        nameof(UserInfoStatus.Blocked) => UserInfoStatus.Blocked,
        var _ => throw new InvalidUserInfoStatusException(newStatus),
    };
}