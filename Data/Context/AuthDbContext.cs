using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Context

{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions options) : base(options)
        {}

        public DbSet<User> Users => Set<User>();
    }
}