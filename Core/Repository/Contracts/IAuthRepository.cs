using Data.DataTransfer;
using Data.Entities;

namespace Core.Repository.Contracts
{
    public interface IAuthRepository
    {
        Task<User> FindUserByNameAsync(string username);

        Task AddUserAsync(User user);

        Task<bool> IsPresent(string username);
    }
}