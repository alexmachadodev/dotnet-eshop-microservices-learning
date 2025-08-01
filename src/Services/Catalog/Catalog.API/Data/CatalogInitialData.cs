using Marten.Schema;

namespace Catalog.API.Data;

public class CatalogInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        await using var session = store.LightweightSession();

        if (await session.Query<Product>().AnyAsync(cancellation))
            return;

        session.Store(GetPreconfiguredProducts());
        
        await session.SaveChangesAsync(cancellation);
    }

    private static IEnumerable<Product> GetPreconfiguredProducts() =>     [
        new()
        {
            Id = Guid.NewGuid(),
            Name = "Apple iPhone 14 Pro",
            Description = "The latest iPhone with advanced features.",
            ImageFile = "iphone14.png",
            Price = 999.99m,
            Category = ["Electronics"]
        },
        new()
        {
            Id = Guid.NewGuid(),
            Name = "Samsung Galaxy S23",
            Description = "A high-end smartphone with a stunning display.",
            ImageFile = "galaxyS23.png",
            Price = 899.99m,
            Category = ["Electronics"]
        },
        new()
        {
            Id = Guid.NewGuid(),
            Name = "Sony WH-1000XM5",
            Description = "Premium noise-canceling headphones.",
            ImageFile = "sonyHeadphones.png",
            Price = 349.99m,
            Category = ["Audio"]
        }
    ];
}