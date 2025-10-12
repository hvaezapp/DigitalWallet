using Carter;
using DigitalWallet.Features.UserWallet.Common;
using Microsoft.AspNetCore.Mvc;

namespace DigitalWallet.Features.UserWallet.Banned;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app
          .MapGroup(FeatureManager.Prefix)
          .WithTags(FeatureManager.EndpointTagName)
          .MapPatch("/{wallet_id:guid:required}/ban",
          async ([FromRoute(Name = "wallet_id")] Guid Id, WalletService service, CancellationToken cancellationToken) =>
          {
              var walletId = WalletId.Create(Id);
              await service.BannedAsync(walletId, cancellationToken);

              return Results.Ok("Wallet banned successfully!");
          });
    }
}