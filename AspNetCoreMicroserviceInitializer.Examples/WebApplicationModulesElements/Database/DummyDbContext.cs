using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Database;

[AutoRegisterDbContext]
public class DummyDbContext : DbContext
{
    private readonly DummyDbContextSettings _dbContextSettings;

    public DbSet<DummyModel> DummyModels { get; set; }

    public DummyDbContext(
        DbContextOptions<DummyDbContext> options,
        IOptions<DummyDbContextSettings> dbContextSettings) : base(options)
    {
        _dbContextSettings = dbContextSettings.Value;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            _dbContextSettings.ConnectionString,
            b => b.MigrationsHistoryTable(_dbContextSettings.MigrationsTableName, _dbContextSettings.MigrationsSchema));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(_dbContextSettings.Schema);
    }
}
