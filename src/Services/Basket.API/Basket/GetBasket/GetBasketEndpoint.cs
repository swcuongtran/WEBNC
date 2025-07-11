namespace Basket.API.Basket.GetBasket
{
    public record GetBasketResponse(ShoppingCart ShoppingCart);
    public class GetBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/basket/{userName}", async (string userName, ISender sender) =>
            {
                var result = await sender.Send(new GetBasketQuery(userName));
                var response = result.Adapt<GetBasketResponse>();
                return Results.Ok(response);
            })
                .WithName("GetBasket")
                .WithSummary("Get a user's shopping basket")
                .WithDescription("Retrieves the shopping basket for a specified user.")
                .Produces<GetBasketResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .WithTags("Basket");
        }
    }
}
