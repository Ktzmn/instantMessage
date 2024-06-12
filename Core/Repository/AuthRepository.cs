using Core.Repository.Contracts;
using Data.Context;
using Data.DataTransfer;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Repository
{
    public class AuthRepository : IAuthRepository
    {   
        private readonly AuthDbContext _context;

        public AuthRepository(AuthDbContext context)
        {
            _context = context; 
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> FindUserByNameAsync(string username)
        {
            return await _context.Users
                    .Where(e => e.Username.Equals(username))
                    .FirstAsync();
        }

        public  async Task<bool> IsPresent(string username)
        {
            return await _context.Users
                .Where(e => e.Username.Equals(username))
                .AnyAsync();
        }
    }
}