using DigitalWallet.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Configuration;

public static class ServiceConfiguration
{
    public static void ConfigureDbContexts(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<WalletDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString(WalletDbContextSchema.DefaultConnectionStringName));
        });

        builder.Services.AddDbContext<WalletDbContextReadOnly>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString(WalletDbContextSchema.DefaultReadOnlyConnectionStringName))
                   .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
    }

}
