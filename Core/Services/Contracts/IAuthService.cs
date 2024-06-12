using Data.DataTransfer;
using Data.Entities;
using Data.Utilities;

namespace Core.Services.Contracts
{
    public interface IAuthService 
    {
        Task<RequestResult> RegisterUserAsync(CreateUserRequest request);

        Task<RequestResult> AuthenticateAsync(AuthUserRequest request);

    }
}