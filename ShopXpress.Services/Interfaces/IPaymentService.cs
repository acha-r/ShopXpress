using System;
using ShopXpress.Services.DTOs.Requests;
using ShopXpress.Services.DTOs.Responses;

namespace ShopXpress.Services.Interfaces
{
    public interface IPaymentService
    {
        
        Task<string> MakePayment(PaymentRequest payment);
        Task<PaystackTransactionResponse> VerifyPayment(string reference);
    }
}

