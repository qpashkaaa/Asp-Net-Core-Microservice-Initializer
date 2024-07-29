using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AspNetCoreMicroserviceInitializer.TestApi.TestElements.Database;

[AutoRegisterDbContext]
public class TestDbContext : DbContext
{
    private readonly TestDbContextSettings _dbContextSettings;

    public DbSet<TestTable> TestTable { get; set; }

    public TestDbContext(
        DbContextOptions<TestDbContext> options,
        IOptions<TestDbContextSettings> dbContextSettings) : base(options)
    {
        _dbContextSettings = dbContextSettings.Value;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "Host=localhost:5432; Database=postgres; Username=postgres; Password=admin",
            b => b.MigrationsHistoryTable(_dbContextSettings.MigrationsTableName, _dbContextSettings.MigrationsSchema));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(_dbContextSettings.Schema);
    }
}
