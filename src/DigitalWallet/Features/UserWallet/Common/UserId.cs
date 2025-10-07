namespace DigitalWallet.Features.UserWallet.Common;

public record UserId(Guid Value)
{
    public static UserId CreateUniqueId() => new(Guid.NewGuid());

    public static UserId Create(Guid value) => new(value);

    public override string ToString()
    {
        return Value.ToString();
    }
};