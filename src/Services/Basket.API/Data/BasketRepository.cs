﻿namespace Basket.API.Data
{
    public class BasketRepository(IDocumentSession session) : IBasketRepository
    {
        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken)
        {
            session.Delete<ShoppingCart>(userName);
            await session.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken)
        {
            var basket = await session.LoadAsync<ShoppingCart>(userName,cancellationToken);
            return basket is null ? throw new BasketNotFoundException(userName) : basket;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart cart, CancellationToken cancellationToken)
        {
            session.Store(cart);
            await session.SaveChangesAsync(cancellationToken);
            return cart;
        }
    }
}
