using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopXpress.Models.Entities;
using ShopXpress.Services.Interfaces;
using System.Xml.Linq;
using ShopXpress.Services.DTOs.Responses;
using ShopXpress.Services.DTOs.Requests;
using System.Security.Claims;

namespace ShopXpress.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartController(ICartService cartService, IHttpContextAccessor httpContextAccessor)
        {
            _cartService = cartService;
            _httpContextAccessor = httpContextAccessor;
        }


        //[HttpGet("get-products")]
        //public async Task<IEnumerable<Product>> GetProducts()
        //{
        //    var products = await _productService.GetProducts();
        //    return products;
        //}


        [HttpPost("add-product-to-cart", Name = "add-product-to-cart")]
        public async Task<CartResponse> CreateCart([FromBody] AddToCartRequest request)
        {


            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var response = await _cartService.CreateCart(userId, request.ProductId, request.Quantity);
            return response;
        }


        //    [HttpPut("update-product/{Id}")]
        //    public async Task<Product> UpdateProduct(string Id, [FromBody] Product product)
        //    {
        //        var updatedProduct = await _productService.UpdateProduct(Id, product);
        //        return updatedProduct;
        //    }

        //    [HttpDelete("delete-product/{Id}")]
        //    public async Task DeleteProduct(string Id)
        //    {
        //        await _productService.DeleteProduct(Id);
        //        return;
        //    }

        //    [HttpGet("search/{query}")]
        //    public async Task<IEnumerable<Product>> Search(string query)
        //    {
        //        var searchProduct = await _productService.Search(query);
        //        return searchProduct;
        //    }
    }

}

