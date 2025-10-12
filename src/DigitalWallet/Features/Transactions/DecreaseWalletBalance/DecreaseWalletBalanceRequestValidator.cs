using FluentValidation;

namespace DigitalWallet.Features.Transactions.DecreaseWalletBalance;

public class DecreaseWalletBalanceRequestValidator : AbstractValidator<DecreaseWalletBalanceRequest>
{
    public DecreaseWalletBalanceRequestValidator()
    {
        RuleFor(x => x.Amount)
         .GreaterThan(0);

        RuleFor(x => x.Description)
            .NotEmpty()
            .NotNull()
            .MaximumLength(500);
    }
}