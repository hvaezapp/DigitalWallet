using Carter;
using DigitalWallet.Infrastructure.Persistence.Context;
using EFCoreSecondLevelCacheInterceptor;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Features.MultiCurrency.GetAll;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app
           .MapGroup(FeatureManager.Prefix)
           .WithTags(FeatureManager.EndpointTagName)
           .MapGet("/",
           async (WalletDbContextReadOnly dbContext, CancellationToken cancellationToken) =>
           {
               var currencies = await GetCurrencies(dbContext, cancellationToken);

               return Results.Ok(currencies);
           });
    }

    public static async Task<List<GetCurrencyResponse>> GetCurrencies(WalletDbContextReadOnly dbContext, CancellationToken cancellationToken)
    {
        var currencies = await dbContext.GetCurrencies()
                .OrderByDescending(x => x.Name)
                .Select(x => new GetCurrencyResponse(x.Id.ToString(), x.Name, x.Code, x.Ratio))
                .Cacheable()
                .ToListAsync(cancellationToken);

        return currencies;
    }
}
