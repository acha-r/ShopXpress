using System;
using ShopXpress.Models.Entities;
using ShopXpress.Services.DTOs.Responses;

namespace ShopXpress.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartItem> GetCartItemsInCart(string cartId);
        Task<CartResponse> CreateCart(string userId, string productId, int quantity);
        Task<CartResponse> RemoveCartItemFromCart(string cartItemId);
        Task<CartResponse> ClearCart(string cartId);
        Task<CartResponse> GetTotalCartPrice(string cartId);
        
    }
}

