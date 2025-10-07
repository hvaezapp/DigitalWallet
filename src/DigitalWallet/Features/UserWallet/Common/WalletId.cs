namespace DigitalWallet.Features.UserWallet.Common;

public record WalletId(Guid Value)
{
    public static WalletId CreateUniqueId() => new(Guid.NewGuid());

    public static WalletId Create(Guid value) => new(value);

    public override string ToString()
    {
        return Value.ToString();
    }
};
