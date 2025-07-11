
namespace Basket.API.Basket.DeleteBasket
{
    public record DeleteBasketResponse(bool IsSuccess);
    public class DeleteBasketEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket/{userName}", async (string userName, ISender sender) =>
            {
                var result = await sender.Send(new DeleteBasketCommand(userName));
                var response = result.Adapt<DeleteBasketResponse>();
                return Results.Ok(response);
            })
                .WithName("DeleteBasket")
                .WithSummary("Delete a user's shopping basket")
                .WithDescription("Deletes the shopping basket for a specified user.")
                .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .WithTags("Basket");
        }
    }
}
