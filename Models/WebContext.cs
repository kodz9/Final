using Microsoft.EntityFrameworkCore;

namespace Final.Models
{
    public class WebContext :DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        public DbSet<Department> Departments { get; set; }

        public WebContext(DbContextOptions<WebContext> options) : base(options)
        {
        }
        public const string DbPath = @".\yzy.db";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }
    }
}
