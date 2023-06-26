using System;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace ShopXpress.Models.Entities
{
    [CollectionName("merchants")]
    public class ApplicationUser : MongoIdentityUser<Guid>
    {
       public string FullName { get; set; } 
    }
}

