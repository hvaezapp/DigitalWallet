using DigitalWallet.Features.MultiCurrency.Common;
using DigitalWallet.Features.Transactions.Common;
using DigitalWallet.Features.UserWallet.Common;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Infrastracture.Persistence.Context;

public class WalletDbContextReadOnly(DbContextOptions<WalletDbContextReadOnly> dbContextOptions) : DbContext(dbContextOptions)
{
    public IQueryable<Transaction> GetTransactions() => Set<Transaction>().AsQueryable();

    public IQueryable<Wallet> GetWallets() => Set<Wallet>().AsQueryable();

    public IQueryable<Currency> GetCurrencies() => Set<Currency>().AsQueryable();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(WalletDbContextSchema.DefaultSchema);

        var assembly = typeof(IAssemblyMarker).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
    }

    public override int SaveChanges()
    {
        throw new InvalidOperationException("This context is read-only.");
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException("This context is read-only.");
    }
}
