using Microsoft.EntityFrameworkCore;

namespace LoginAPI.Data
{
    public class LoginDbContext : DbContext
    {
        public LoginDbContext(DbContextOptions options) : base(options)
        {
        }

        //DbContext
        public DbSet<Login> Logins { get; set; }
    }
}
