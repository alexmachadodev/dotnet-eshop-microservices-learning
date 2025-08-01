namespace Basket.API.Basket.GetBasket;

public record GetBasketResponse(ShoppingCart? Cart);

public class GetBasketEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{userName}", async (string userName, ISender sender) =>
            {
                var result = await sender.Send(new GetBasketQuery(userName));

                return Results.Ok(new GetBasketResponse(result.Cart));
            })
            .WithName("GetBasket")
            .Produces<GetBasketResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get a user's shopping cart")
            .WithDescription("Retrieves the shopping cart for a specified user.");
    }
}