using DigitalWallet.Features.UserWallet.Common;

namespace DigitalWallet.Features.Transactions.Common;

public class Transaction
{
    public TransactionId Id { get; private set; } = null!;

    public WalletId WalletId { get; private set; } = null!;

    public Wallet Wallet { get; private set; } = null!;

    public string Description { get; private set; } = string.Empty;

    public decimal Amount { get; private set; }

    public DateTime CreatedOnUtc { get; private set; }

    public TransactionKind Kind { get; private set; }

    public TransactionType Type { get; private set; }

    
}