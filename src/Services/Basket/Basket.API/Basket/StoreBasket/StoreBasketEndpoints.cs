namespace Basket.API.Basket.StoreBasket;

public record StoreBasketRequest(ShoppingCart Cart);
public record StoreBasketResponse(string UserName);

public class StoreBasketEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", async (StoreBasketRequest request, ISender sender) =>
            {
                var result = await sender.Send(new StoreBasketCommand(request.Cart));

                var response = new StoreBasketResponse(result.UserName);

                return Results.Created($"/basket/{response.UserName}", new StoreBasketResponse(result.UserName));
            })
            .WithName("StoreBasket")
            .Produces<StoreBasketResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Store a user's shopping cart")
            .WithDescription(
                "Stores the shopping cart for a specified user. If the cart already exists, it updates the existing cart.");
    }
}