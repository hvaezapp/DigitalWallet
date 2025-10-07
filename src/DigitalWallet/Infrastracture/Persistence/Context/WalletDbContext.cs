using DigitalWallet.Features.MultiCurrency.Common;
using DigitalWallet.Features.Transactions.Common;
using DigitalWallet.Features.UserWallet.Common;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Infrastracture.Persistence.Context;

public class WalletDbContext(DbContextOptions<WalletDbContext> dbContextOptions) : DbContext(dbContextOptions)
{
    public DbSet<Wallet> Wallets => Set<Wallet>();
    public DbSet<Currency> Currencies => Set<Currency>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(WalletDbContextSchema.DefaultSchema);

        var assembly = typeof(IAssemblyMarker).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
    }
}
