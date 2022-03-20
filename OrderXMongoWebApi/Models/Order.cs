using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace OrderXMongoWebApi.Models
{
    [BsonIgnoreExtraElements]
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? _id { get; set; }
        public string EventId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string[] Addons { get; set; }
        public OrderItem[] OrderItems { get; set; }
        public DateTime DateOpened { get; set; }
        public DateTime DateClosed { get; set; }
        public bool  Active { get; set; }
        public string WaiterUserName { get; set; }
        public string TableId { get; set; }
        public string TableName { get; set; }
    }

}
