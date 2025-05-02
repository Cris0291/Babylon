using Babylon.Common.Application.Messaging;
using Babylon.Common.Domain;
using Babylon.Modules.Users.Application.Abstractions.Data;
using Babylon.Modules.Users.Application.Abstractions.Identity;
using Babylon.Modules.Users.Domain.Users;

namespace Babylon.Modules.Users.Application.Users.RegisterUser;
internal sealed class RegisterUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IIdentityProviderService identityProviderService) : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        Result<string> result = await identityProviderService.RegisterUserAsync(request.Email, request.FirstName, request.LastName, request.Password, cancellationToken);

        if (!result.IsSuccess)
        {
            return Result.Failure<Guid>(result.Error!);
        }
        var user = User.Create(request.FirstName, request.LastName, request.Email, result.TValue!);

        await userRepository.Insert(user);
          
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
