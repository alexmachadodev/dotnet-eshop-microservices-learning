using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Data;

public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache) : IBasketRepository
{
    public async Task<ShoppingCart?> GetBasket(string userName, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"basket-{userName}";

        var cachedBasket = await cache.GetStringAsync(cacheKey, cancellationToken);

        if (cachedBasket is not null)
        {
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket);
        }

        var basket = await repository.GetBasket(userName, cancellationToken);

        await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(basket), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        }, cancellationToken);

        return basket;
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart shoppingCart, CancellationToken cancellationToken = default)
    {
        await repository.StoreBasket(shoppingCart, cancellationToken);

        var cacheKey = $"basket-{shoppingCart.UserName}";

        await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(shoppingCart), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        }, cancellationToken);

        return shoppingCart;
    }

    public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
    {
        await repository.DeleteBasket(userName, cancellationToken);

        var cacheKey = $"basket-{userName}";

        await cache.RemoveAsync(cacheKey, cancellationToken);
        
        return true;
    }
}