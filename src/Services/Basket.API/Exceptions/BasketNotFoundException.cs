﻿namespace Basket.API.Exceptions
{
    public class BasketNotFoundException : NotFoundException
    {
        public BasketNotFoundException(string UserName) : base("basket", UserName)
        {
            
        }
    }
}
