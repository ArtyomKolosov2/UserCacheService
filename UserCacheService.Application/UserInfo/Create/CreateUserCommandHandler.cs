using MediatR;
using UserCacheService.Domain.Exceptions;
using UserCacheService.Domain.UserInfo.Repository;

namespace UserCacheService.Application.UserInfo.Create;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Domain.UserInfo.UserInfo>
{
    private readonly IUserInfoRepository _userInfoRepository;

    public CreateUserCommandHandler(IUserInfoRepository userInfoRepository)
    {
        _userInfoRepository = userInfoRepository;
    }
    
    public async Task<Domain.UserInfo.UserInfo> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userInfoRepository.Contains(request.UserInfo.Id, cancellationToken))
            throw new UserInfoAlreadyExistsException(request.UserInfo.Id);
        
        return await _userInfoRepository.Create(request.UserInfo, cancellationToken);
    }
}