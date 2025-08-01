namespace Catalog.API.Products.CreateProduct;

public record CreateProductRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);

public record CreateProductResponse(Guid Id);

public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products",
                async (CreateProductRequest request, ISender sender) =>
                {
                    var command = new CreateProductCommand(request.Name, request.Category, request.Description,
                        request.ImageFile, request.Price);

                    var result = await sender.Send(command, CancellationToken.None);

                    var responde = new CreateProductResponse(result.Id);

                    return Results.Created($"/products/{responde.Id}", responde);
                })
            .WithName("CreateProduct")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .WithSummary("Create Product")
            .WithDescription("Create a new product in the catalog.");
    }
}