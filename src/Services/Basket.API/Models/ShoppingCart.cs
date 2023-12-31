﻿namespace Basket.API.Models
{
    public class ShoppingCart
    {
        public string UserName { get; set; }
        public List<ShoppingCartItem> Items { get; set; }

        public ShoppingCart()
        {
            Items = new List<ShoppingCartItem>();
        }
        public ShoppingCart(string userName)
        {
            UserName = userName;
            Items = new List<ShoppingCartItem>();
        }

        public decimal TotalPrice 
        { 
            get 
            { 
                decimal totalPrice = 0;
                foreach (var item in Items)
                {
                    totalPrice += (item.Price * item.Quantity);
                }
                return totalPrice;
            } 
        }
    }
}
