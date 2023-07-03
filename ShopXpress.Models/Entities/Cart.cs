using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace ShopXpress.Models.Entities
{
    public class Cart
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("status")]
        public bool isActive { get; set; } = true;
        [BsonElement("sessionId")]
        public string SessionId { get; set; }
        [BsonElement("userId")]
        public string UserId { get; set; }
        [BsonElement("items")]
        public List<CartItem> CartItems { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

