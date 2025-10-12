using System.Diagnostics.CodeAnalysis;

namespace DigitalWallet.Features.UserWallet.Common;

public class WalletNotActiveException : Exception
{
    private const string _message = "Wallet with ID {0} not active current staus is {1}.";
    public WalletNotActiveException(WalletId walletId , WalletStatus status) : base(string.Format(_message, walletId , status.ToString().ToLower()))
    {
    }

    [DoesNotReturn]
    public static void Throw(WalletId walletId, WalletStatus status)
    {
        throw new WalletNotActiveException(walletId , status);
    }
}
