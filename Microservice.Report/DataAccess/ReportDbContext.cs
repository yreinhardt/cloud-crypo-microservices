using Microsoft.EntityFrameworkCore;

namespace Microservice.Report.DataAccess;

// db abstraction
public class ReportDbContext : DbContext
{
    public ReportDbContext() { }
    public ReportDbContext(DbContextOptions opts) : base(opts) { }
    public DbSet<PortfolioReport> Report { get; set; }

    // override default behavior to snakecase table names
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        SnakeCaseIdentitTableNames(modelBuilder);
    }

    private static void SnakeCaseIdentitTableNames(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Report>(b => { b.ToTable("report"); });
    }
}
