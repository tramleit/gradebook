using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Gradebook.Permissions.Database;

public class PermissionsDatabaseContext: DbContext
{
    public PermissionsDatabaseContext()
    {
        
    }
    public PermissionsDatabaseContext(DbContextOptions<PermissionsDatabaseContext> options)
        : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        if (!optionsBuilder.IsConfigured)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var cfg = builder.Build();
            var cn = cfg.GetConnectionString("DefaultAppDatabase");
            optionsBuilder.UseMySql(
                cn,
                new MySqlServerVersion(new Version(8, 30, 0)),
                e=>e.MigrationsHistoryTable("__PermissionsMigrationsHistory")
            );
        }
    }
}
