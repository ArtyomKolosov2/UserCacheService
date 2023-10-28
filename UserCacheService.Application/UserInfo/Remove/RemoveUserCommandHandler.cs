using MediatR;
using UserCacheService.Domain.Exceptions;
using UserCacheService.Domain.UserInfo.Repository;

namespace UserCacheService.Application.UserInfo.Remove;

public class RemoveUserCommandHandler : IRequestHandler<RemoveUserCommand, Domain.UserInfo.UserInfo>
{
    private readonly IUserInfoRepository _userInfoRepository;

    public RemoveUserCommandHandler(IUserInfoRepository userInfoRepository)
    {
        _userInfoRepository = userInfoRepository;
    }
    
    public async Task<Domain.UserInfo.UserInfo> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
    {
        var deletedObject = await _userInfoRepository.DeleteById(request.Id, cancellationToken);

        if (deletedObject is null)
            throw new UserInfoNotFoundException(request.Id);

        return deletedObject;
    }
}