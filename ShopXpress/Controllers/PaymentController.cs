using System;
using Microsoft.AspNetCore.Mvc;
using ShopXpress.Models.Entities;
using ShopXpress.Services.DTOs.Requests;
using ShopXpress.Services.DTOs.Responses;
using ShopXpress.Services.Implementations;
using ShopXpress.Services.Interfaces;

namespace ShopXpress.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class PaymentController : Controller
   {
       private readonly IPaymentService _paymentService;

       public PaymentController(IPaymentService paymentService)
       {
          _paymentService = paymentService;
       }

       [HttpPost("make-payment", Name = "make-payment")]
       public async Task<string> MakePayment([FromBody] PaymentRequest payment)
       {
          var response = await _paymentService.MakePayment(payment);
          return response;
       }

       [HttpGet("payment-verification", Name = "payment-verification")]
       public async Task<PaystackTransactionResponse> VerifyPayment(string reference)
       {
          var response = await _paymentService.VerifyPayment(reference);
          return response;
       }
    }
}

