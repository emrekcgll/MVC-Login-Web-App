using Microsoft.EntityFrameworkCore;

namespace Login.Models
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions options): base(options) {}
        public DbSet<User> Users { get; set; }
        public DbSet<PasswordCode> PasswordCodes { get; set; }

    }
}
