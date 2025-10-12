using DigitalWallet.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.Features.MultiCurrency.Common;

public class CurrencyService(WalletDbContext dbContext)
{
    private readonly WalletDbContext _dbContext = dbContext;

    public async Task<CurrencyId> CreateAsync(string code, string name, decimal ratio, CancellationToken ct)
    {
        if (await _dbContext.Currencies.AnyAsync(x => x.Code == code, ct))
        {
            DuplicateCurrencyException.Throw(code);
        }

        if (ratio == 0)
        {
            InvalidCurrencyRatioException.Throw();
        }

        var currency = Currency.Create(code, name, ratio);

        _dbContext.Currencies.Add(currency);
        await _dbContext.SaveChangesAsync(ct);

        return currency.Id;
    }

    public async Task UpdateRationAsync(CurrencyId currencyId, decimal ratio, CancellationToken ct)
    {
        if (ratio == 0)
        {
            InvalidCurrencyRatioException.Throw();
        }

        var currency = await _dbContext.Currencies.FirstOrDefaultAsync(x => x.Id == currencyId, ct);
        if (currency is null)
        {
            CurrencyNotFoundException.Throw(currencyId);
        }

        currency.UpdateRatio(ratio);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task<bool> IsCurrencyIdValidAsync(CurrencyId currencyId, CancellationToken ct)
    {
        return await _dbContext.Currencies.AnyAsync(x => x.Id == currencyId, ct);
    }

}
