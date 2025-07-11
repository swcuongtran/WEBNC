namespace Basket.API.Basket.GetBasket
{
    public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;
    public record GetBasketResult(ShoppingCart ShoppingCart);
    public class GetBasketHandler : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        public Task<GetBasketResult> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {
            // Logic to retrieve the basket for the user
            // This is a placeholder implementation
            var shoppingCart = new ShoppingCart(); // Retrieve from database or cache
            return Task.FromResult(new GetBasketResult(shoppingCart));
        }
    }
}
