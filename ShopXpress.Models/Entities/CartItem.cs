using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ShopXpress.Models.Entities
{
    public class CartItem
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("userId")]
        public string UserId { get; set; }
        [BsonElement("cartId")]
        public string CartId { get; set; }
        [BsonElement("productId")]
        public string ProductId { get; set; }
        [BsonElement("quantity")]
        public int Quantity { get; set; }
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [BsonElement("updatedAt")]
        public string? UpdatedAt { get; set; }
    }
}

