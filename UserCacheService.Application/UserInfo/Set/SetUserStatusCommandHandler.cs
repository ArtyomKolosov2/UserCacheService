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

        userInfo.Status = Enum.Parse<UserInfoStatus>(request.Status);
        var updatedUserInfo = await _userInfoRepository.Update(userInfo, cancellationToken);
        
        return updatedUserInfo;
    }
}