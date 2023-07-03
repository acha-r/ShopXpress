using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using PayStack.Net;
using ShopXpress.Models.Entities;
using ShopXpress.Services.DTOs.Responses;
using ShopXpress.Services.Interfaces;

namespace ShopXpress.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly IMongoCollection<CartItem> _cartItemService;
        private readonly IMongoCollection<Product> _productService;
        private readonly IMongoCollection<Cart> _cartService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(IMongoDatabase database, IHttpContextAccessor httpContextAccessor)
        {
            _cartItemService = database.GetCollection<CartItem>("cartItem");
            _productService = database.GetCollection<Product>("products");
            _cartService = database.GetCollection<Cart>("cart");
            _httpContextAccessor = httpContextAccessor;
           
        }

        //public async Task<CartResponse> CreateCart(string userId, string productId, int quantity)
        //{
        //    var sessionID = _httpContextAccessor.HttpContext.Session.Id;
        //    var existingCart = _cartService.Find(cart => cart.SessionId == sessionID || cart.UserId == userId).FirstOrDefault();
        //    if (existingCart == null)
        //    {

        //        var existingProduct = _productService.Find(product => product.Id == productId).FirstOrDefault();

        //        if (existingProduct is null)
        //            return new CartResponse
        //            {
        //                Success = false,
        //                Message = "Product either does not exist or has been deleted"
        //            };

        //        if (existingProduct.UserId == userId)
        //            return new CartResponse
        //            {
        //                Message = "You cannot buy your own products",
        //                Success = false
        //            };

        //        if (existingProduct.Quantity < quantity)
        //            return new CartResponse
        //            {
        //                Message = $"OOPS! Insufficient Products\nWe currently have {existingProduct.Quantity} of {existingProduct.Name} in our store, " +
        //                $"\nyour order exceeds that! \nReduce your order and try again \nor check again in a few days after restock",
        //                Success = false
        //            };

        //        Cart cart = new Cart();
        //        var cartItems = new CartItem
        //        {

        //            UserId = userId,
        //            ProductId = productId,
        //            Quantity = quantity,
        //            CartId = cart.Id,
        //            CreatedAt = DateTime.Now

        //        };
        //        _cartItemService.InsertOne(cartItems);

        //        cart.SessionId = sessionID;
        //        cart.UserId = userId;
        //        cart.CartItems.Add(cartItems);
        //        _cartService.InsertOne(cart);

        //        return new CartResponse
        //        {
        //            Message = "dhdhdhj",
        //            Success = true
        //        };
        //    }

            

            //var existingProduct = _productService.Find(product => product.Id == productId ).FirstOrDefault();

        //if (existingProduct is null)
        //    return new CartResponse
        //    {
        //        Success = false,
        //        Message = "Product either does not exist or has been deleted"
        //    };

        //if (existingProduct.UserId == userId)
        //    return new CartResponse
        //    {
        //        Message = "You cannot buy your own products",
        //        Success = false
        //    };

        //if (existingProduct.Quantity < quantity)
        //    return new CartResponse
        //    {
        //        Message = $"OOPS! Insufficient Products\nWe currently have {existingProduct.Quantity} of {existingProduct.Name} in our store, " +
        //        $"\nyour order exceeds that! \nReduce your order and try again \nor check again in a few days after restock",
        //        Success = false
        //    };


        //  var cart = _cartService.Find(c => c.UserId == userId).FirstOrDefault();
        //var cartItem = _cartItemService.Find(x => x.Id.ToString() == cartItemId && x.ProductId.ToString() == productId).FirstOrDefault();


        //    var cartItem = _cartItemService.Find(p => p.ProductId == productId).FirstOrDefault();
        //    var newQty = cartItem.Quantity + quantity;
            
        //    if (cartItem is not null)
        //    {
        //        existingCart.CartItems.Remove(cartItem);

        //        cartItem.Quantity += quantity;
        //        var productFilter = Builders<CartItem>.Filter.Eq("ProductId", cartItem.ProductId);
        //        var update = Builders<CartItem>.Update.Set("Quantity", newQty);
        //        await _cartItemService.UpdateOneAsync(productFilter, update);

        //        existingCart.CartItems.Add(cartItem);

        //        _cartService.ReplaceOne(x => x.Id == existingCart.Id, existingCart);
                

        //        return new CartResponse
        //        { Success = true,
        //          Message = $"Cart item: Quantity increased"
        //        };
        //    }

     

        //    return new CartResponse
        //    {
        //        Success = true,
        //        Message = $"{quantity} unit(s) of item Added"
        //    };
        //}

        public async Task<CartResponse> CreateCart(string userId, string productId, int quantity)
        {
            var sessionID = _httpContextAccessor.HttpContext.Session.Id;
            var existingCart = _cartService.Find(cart => cart.UserId == userId).FirstOrDefault();

            if (existingCart == null)
            {

                var existingProduct = _productService.Find(product => product.Id == productId).FirstOrDefault();

                if (existingProduct is null)
                    return new CartResponse
                    {
                        Success = false,
                        Message = "Product either does not exist or has been deleted"
                    };

                if (existingProduct.UserId == userId)
                    return new CartResponse
                    {
                        Message = "You cannot buy your own products",
                        Success = false
                    };

                if (existingProduct.Quantity < quantity)
                    return new CartResponse
                    {
                        Message = $"OOPS! Insufficient Products\nWe currently have {existingProduct.Quantity} of {existingProduct.Name} in our store, " +
                        $"\nyour order exceeds that! \nReduce your order and try again \nor check again in a few days after restock",
                        Success = false
                    };


                var cart = new Cart()
                {
                    UserId = userId,
                    SessionId = sessionID,
                    CartItems = new List<CartItem>()
                };

                var cartItem = new CartItem
                {

                    UserId = userId,
                    CartId = cart.Id,
                    Quantity = quantity,
                    ProductId = productId
                };

                cart.CartItems.Add(cartItem);

                await _cartService.InsertOneAsync(cart);

                return new CartResponse
                {
                    Message = $"cart has been created with {quantity} quantity of {existingProduct.Name}",
                    Success = true
                };
            }

            

                var cartProduct = _cartItemService.Find(item => item.ProductId == productId).FirstOrDefault();

                if (cartProduct != null)
                {
                  cartProduct.Quantity += quantity;
                  var productFilter = Builders<Cart>.Filter.Eq("productId", cartProduct.ProductId);
                  var update = Builders<Cart>.Update.Set("quantity", cartProduct.Quantity);
                   await _cartService.UpdateOneAsync(productFilter, update);

                    return new CartResponse
                    {
                        Success = true,
                        Message = $"Cart item: Quantity increased"
                    };
                
                }


            return new CartResponse
            {
                Success = true,
                Message = $"{quantity} unit(s) of item Added"
            };

        }

        public Task<CartResponse> ClearCart(string cartId)
        {
            throw new NotImplementedException();
        }

        public Task<CartItem> GetCartItemsInCart(string cartId)
        {
            throw new NotImplementedException();
        }

        public Task<CartResponse> GetTotalCartPrice(string cartId)
        {
            throw new NotImplementedException();
        }

        public Task<CartResponse> RemoveCartItemFromCart(string cartItemId)
        {
            throw new NotImplementedException();
        }
    }
}

