using Microsoft.EntityFrameworkCore;

namespace Microservice.User.DataAccess;

// db abstraction
public class UserDbContext : DbContext
{
    public UserDbContext() { }
    public UserDbContext(DbContextOptions opts) : base(opts) { }
    public DbSet<User> User { get; set; }

    // override default behavior to snakecase table names
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        SnakeCaseIdentitTableNames(modelBuilder);
    }

    private static void SnakeCaseIdentitTableNames(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(b => { b.ToTable("user"); });
    }
}
