using DigitalWallet.Features.MultiCurrency.Common;
using DigitalWallet.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Features.UserWallet.Common;

public class WalletService(CurrencyService currencyService, WalletDbContext dbContext)
{
    private readonly CurrencyService _currencyService = currencyService;
    private readonly WalletDbContext _dbContext = dbContext;

    public async Task<WalletId> CreateAsync(UserId userId, CurrencyId currencyId, string title, CancellationToken ct)
    {
        if (!await _currencyService.IsCurrencyIdValidAsync(currencyId, ct))
        {
            InvalidCurrencyException.Throw(currencyId);
        }

        if (await _dbContext.Wallets.AnyAsync(x => x.UserId == userId && x.CurrencyId == currencyId, ct))
        {
            WalletAlreadyExistsException.Throw(userId, currencyId);
        }

        var wallet = Wallet.Create(userId, currencyId, title);

        _dbContext.Wallets.Add(wallet);
        await _dbContext.SaveChangesAsync(ct);

        return wallet.Id;
    }

    internal async Task ChangeTitleAsync(WalletId walletId, string title, CancellationToken ct)
    {
        var wallet = await GetWalletAsync(walletId, ct);

        if (!await IsWalletAvailableAsync(walletId, ct))
            WalletUnavailableException.Throw(walletId);

        wallet.UpdateTitle(title);
        await _dbContext.SaveChangesAsync(ct);
    }

    internal async Task ActiveAsync(WalletId walletId, CancellationToken ct)
    {
        var wallet = await GetWalletAsync(walletId, ct);

        wallet.Activate();
        await _dbContext.SaveChangesAsync(ct);
    }

    internal async Task SuspendAsync(WalletId walletId, CancellationToken ct)
    {
        var wallet = await GetWalletAsync(walletId, ct);

        wallet.Suspend();
        await _dbContext.SaveChangesAsync(ct);
    }


    internal async Task BannedAsync(WalletId walletId, CancellationToken ct)
    {
        var wallet = await GetWalletAsync(walletId, ct);

        wallet.Banned();
        await _dbContext.SaveChangesAsync(ct);
    }

    internal async Task<bool> IsWalletAvailableAsync(WalletId walletId, CancellationToken ct)
    {
        var wallet = await GetWalletAsync(walletId, ct);
        return wallet.Status == WalletStatus.Active;
    }

    private async Task<Wallet> GetWalletAsync(WalletId walletId, CancellationToken ct)
    {
        var wallet = await _dbContext.Wallets.FirstOrDefaultAsync(x => x.Id == walletId, ct);

        if (wallet is null)
        {
            WalletNotFoundException.Throw(walletId);
        }

        return wallet;
    }

    internal async Task IncreaseBalanceAsync(WalletId walletId, decimal amount, CancellationToken cancellationToken)
    {
        var wallet = await GetWalletAsync(walletId, cancellationToken);

        wallet.IncreaseBalance(amount);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    internal async Task DecreaseBalanceAsync(WalletId walletId, decimal amount, CancellationToken cancellationToken)
    {
        var wallet = await GetWalletAsync(walletId, cancellationToken);

        if (wallet.Balance - amount < 0)
        {
            InsufficientBalanceException.Throw();
        }

        wallet.DecreaseBalance(amount);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    internal async Task<bool> IsUserOwnedAsync(List<WalletId> WalletIds, CancellationToken ct)
    {
        return (await _dbContext.Wallets.Where(x => WalletIds.Contains(x.Id))
                                            .ToListAsync(ct))
                                            .DistinctBy(x => x.UserId)
                                            .Count() == 1;
    }

    internal async Task<decimal> WalletFundsAsync(WalletId sourceWalletId, WalletId destinationWalletId, decimal amount, CancellationToken cancellationToken)
    {
        var walletSource = await GetWalletAsync(sourceWalletId, cancellationToken);
        var walletDestination = await GetWalletAsync(destinationWalletId, cancellationToken);

        walletSource.DecreaseBalance(amount);

        var destinationAmount = walletSource.Currency.Ratio / walletDestination.Currency.Ratio * amount;

        walletDestination.IncreaseBalance(destinationAmount);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return destinationAmount;
    }

}
