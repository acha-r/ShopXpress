using System;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PayStack.Net;
using ShopXpress.Data.Implementations;
//using ShopXpress.Data.Implementations;
using ShopXpress.Models.Entities;
using ShopXpress.Services.DTOs.Requests;
using ShopXpress.Services.DTOs.Responses;
using ShopXpress.Services.Interfaces;

namespace ShopXpress.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IMongoCollection<Transaction> _transaction;

        private PayStackApi PayStackApi { get; set; }
        private readonly string token;

        public PaymentService(IOptions<DatabaseSettings> mongoDBSettings, IMongoDatabase database)
        {
            _transaction = database.GetCollection<Transaction>("transactions");
            token = mongoDBSettings.Value.PayStackSK;
            PayStackApi = new PayStackApi(token);

        }

        
        public async Task<string> MakePayment(PaymentRequest payment)
        {

            TransactionInitializeRequest request = new()
            {
                AmountInKobo = payment.Amount * 100,
                Email = payment.Email,
                Reference = GenerateReference().ToString(),
                Currency = "NGN",
                CallbackUrl = "http://localhost:24946"
            };

            TransactionInitializeResponse response = PayStackApi.Transactions.Initialize(request);
            if (response.Status)
            {
                var transaction = new Transaction
                {
                    Amount = payment.Amount,
                    Email = payment.Email,
                    Ref = request.Reference,
                    Name = payment.Name

                };
                await _transaction.InsertOneAsync(transaction);

                return response.Data.AuthorizationUrl;
            }
            return null;
        }

        public async Task<PaystackTransactionResponse> VerifyPayment(string reference)
        {
            TransactionVerifyResponse response = PayStackApi.Transactions.Verify(reference);
            if(response.Data.Status == "success")
            {
                var transaction = await _transaction.Find(t => t.Ref == reference).FirstOrDefaultAsync();
                if(transaction != null)
                {
                    transaction.Status = true;
                   await _transaction.UpdateOneAsync(Builders<Transaction>.Filter.Eq(t => t.Name, transaction.Name),
                  Builders<Transaction>.Update.Set(t => t.Status, true));
                    
                }
                var paystackResponse = new PaystackTransactionResponse
                {
                    Success = true,
                    Message = "Transaction verified successfully",
                    
                };

                return paystackResponse;
            }

            return new PaystackTransactionResponse
            {
                Success = false,
                Message = "Transaction verification failed",
            };
        }

        public int GenerateReference()
        {
            Random random = new Random();
            int randomNumber = random.Next(1000000, 9999999);
            return randomNumber;
        }
    }
}

