using Carter;
using DigitalWallet.Common.Extensions;
using DigitalWallet.Features.Transactions.Common;
using DigitalWallet.Features.UserWallet.Common;
using Microsoft.AspNetCore.Mvc;

namespace DigitalWallet.Features.Transactions.DecreaseWalletBalance;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app
            .MapGroup(FeatureManager.Prefix)
            .WithTags(FeatureManager.EndpointTagName)
            .MapPost("/{wallet_id:guid:required}/decreaseBalance",
            async ([FromBody] DecreaseWalletBalanceRequest request, 
                   [FromRoute(Name = "wallet_id")] Guid Id, 
                   TransactionService _service, 
                   CancellationToken cancellationToken) =>
            {
                var walletId = WalletId.Create(Id);
                await _service.DecreaseWalletBalanceAsync(walletId, 
                                                            request.Amount, 
                                                            request.Description,
                                                            cancellationToken);

                return Results.Ok("Wallet balance decreased successfully!");

            }).Validator<DecreaseWalletBalanceRequest>();
    }
}
