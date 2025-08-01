namespace Catalog.API.Products.GetProducts;

//public record GetProductRequest(string? Category = null, string? Name = null, decimal? MinPrice = null, decimal? MaxPrice = null);
public record GetProductsResponse(IEnumerable<Product> Products);
public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (ISender sender) =>
        {
            var result = await sender.Send(new GetProductsQuery(), CancellationToken.None);

            var response = new GetProductsResponse(result.Products);

            return Results.Ok(response);
        })
        .WithName("GetProducts")
        .Produces<GetProductsResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Products")
        .WithDescription("Retrieve a list of products from the catalog.");
    }
}