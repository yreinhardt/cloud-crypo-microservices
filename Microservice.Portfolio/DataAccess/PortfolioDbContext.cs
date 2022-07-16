using Microsoft.EntityFrameworkCore;

namespace Microservice.Portfolio.DataAccess;

// db abstraction
public class PortfolioDbContext : DbContext
{
    public PortfolioDbContext() { }
    public PortfolioDbContext(DbContextOptions opts) : base(opts) { }
    public DbSet<Portfolio> Portfolio { get; set; }

    // override default behavior to snakecase table names
    protected override void OnModelCreating (ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        SnakeCaseIdentitTableNames(modelBuilder);
    }

    private static void SnakeCaseIdentitTableNames (ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Portfolio>(b => { b.ToTable("portfolio"); });
    }
}


