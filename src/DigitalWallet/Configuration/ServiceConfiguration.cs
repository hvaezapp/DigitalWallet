using DigitalWallet.Infrastructure.Persistence.Context;
using EFCoreSecondLevelCacheInterceptor;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Configuration;

public static class ServiceConfiguration
{
    public static IServiceCollection ConfigureDbContexts(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<WalletDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString(WalletDbContextSchema.DefaultConnectionStringName));
        });

        services.AddDbContext<WalletDbContextReadOnly>((serviceProvider, optionsBuilder) =>
        {
            optionsBuilder
                .UseSqlServer(configuration.GetConnectionString(WalletDbContextSchema.DefaultReadOnlyConnectionStringName))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .AddInterceptors(serviceProvider.GetRequiredService<SecondLevelCacheInterceptor>());
        });

        return services;
    }

    public static IServiceCollection ConfigureSecondLevelCache(this IServiceCollection services)
    {
        services.AddEFSecondLevelCache(options =>
                options.UseMemoryCacheProvider().ConfigureLogging(true).UseCacheKeyPrefix("EF_"));

        return services;

    }

    public static IServiceCollection ConfigureValidator(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();

        return services;
    }
}
