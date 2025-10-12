using DigitalWallet.Features.MultiCurrency.Common;
using DigitalWallet.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Features.UserWallet.Common;

public class WalletService(CurrencyService currencyService, WalletDbContext dbContext)
{
    private readonly CurrencyService _currencyService = currencyService;
    private readonly WalletDbContext _dbContext = dbContext;

    public async Task<WalletId> CreateAsync(UserId userId, CurrencyId currencyId, string title, CancellationToken cancellationToken)
    {
        if (!await _currencyService.IsCurrencyIdValidAsync(currencyId, cancellationToken))
        {
            InvalidCurrencyException.Throw(currencyId);
        }

        if (await _dbContext.Wallets.AnyAsync(x => x.UserId == userId && x.CurrencyId == currencyId, cancellationToken))
        {
            WalletAlreadyExistsException.Throw(userId, currencyId);
        }

        var wallet = Wallet.Create(userId, currencyId, title);

        _dbContext.Wallets.Add(wallet);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return wallet.Id;
    }

    internal async Task ChangeTitleAsync(WalletId walletId, string title, CancellationToken cancellationToken)
    {
        var wallet = await GetWalletAsync(walletId, cancellationToken);

        // we can split this logic if needed after
        if (wallet.Status != WalletStatus.Active)
            WalletNotActiveException.Throw(walletId, wallet.Status);

        wallet.UpdateTitle(title);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    internal async Task ActiveAsync(WalletId walletId, CancellationToken cancellationToken)
    {
        var wallet = await GetWalletAsync(walletId, cancellationToken);

        wallet.Activate();
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    internal async Task SuspendAsync(WalletId walletId, CancellationToken cancellationToken)
    {
        var wallet = await GetWalletAsync(walletId, cancellationToken);

        wallet.Suspend();
        await _dbContext.SaveChangesAsync(cancellationToken);
    }


    internal async Task BannedAsync(WalletId walletId, CancellationToken cancellationToken)
    {
        var wallet = await GetWalletAsync(walletId, cancellationToken);

        wallet.Banned();
        await _dbContext.SaveChangesAsync(cancellationToken);
    }


    private async Task<Wallet> GetWalletAsync(WalletId walletId, CancellationToken cancellationToken)
    {
        var wallet = await _dbContext.Wallets
                                     .FirstOrDefaultAsync(x => x.Id == walletId, 
                                      cancellationToken);

        if (wallet is null)
        {
            WalletNotFoundException.Throw(walletId);
        }

        return wallet;
    }

}
