namespace DigitalWallet.Features.Transactions.Common;

public record TransactionId(Guid Value)
{
    public static TransactionId CreateUniqueId() => new(Guid.NewGuid());

    public static TransactionId Create(Guid value) => new(value);

    public override string ToString()
    {
        return Value.ToString();
    }
};