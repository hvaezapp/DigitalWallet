using DigitalWallet.Features.UserWallet.Common;
using DigitalWallet.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Features.Transactions.Common;

public class TransactionService(WalletService walletService , WalletDbContext dbContext) 
{
    private readonly WalletService _walletService = walletService;
    private readonly WalletDbContext _dbContext = dbContext;


    internal async Task IncreaseWalletBalanceAsync(WalletId walletId, decimal amount, string description, CancellationToken ct)
    {
        if (!await _walletService.IsWalletAvailableAsync(walletId, ct))
        {
            WalletUnavailableException.Throw(walletId);
        }

        InvalidTransactionAmountException.Throw(amount);

        var dbTransaction = await _dbContext.Database.BeginTransactionAsync(ct);

        try
        {
            await _walletService.IncreaseBalanceAsync(walletId, amount, ct);

            var transaction = Transaction.CreateIncreaseWalletBalanceTransaction(walletId, amount, description);

            _dbContext.Transactions.Add(transaction);
            await _dbContext.SaveChangesAsync(ct);

            await dbTransaction.CommitAsync(ct);
        }
        catch (Exception)
        {
            await dbTransaction.RollbackAsync(ct);
        }
    }


    internal async Task DecreaseWalletBalanceAsync(WalletId walletId, decimal amount, string description, CancellationToken ct)
    {
        if (!await _walletService.IsWalletAvailableAsync(walletId, ct))
        {
            WalletUnavailableException.Throw(walletId);
        }

        InvalidTransactionAmountException.Throw(amount);

        var dbTransaction = await _dbContext.Database.BeginTransactionAsync(ct);

        try
        {
            await _walletService.DecreaseBalanceAsync(walletId, amount, ct);

            var transaction = Transaction.CreateDecreaseWalletBalanceTransaction(walletId, amount, description);

            _dbContext.Transactions.Add(transaction);
            await _dbContext.SaveChangesAsync(ct);

            await dbTransaction.CommitAsync(ct);
        }
        catch (Exception)
        {
            await dbTransaction.RollbackAsync(ct);
        }
    }
}
