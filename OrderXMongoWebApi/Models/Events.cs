using MongoDB.Bson;
using System;
using MongoDB.Bson.Serialization.Attributes;

namespace OrderXMongoWebApi.Models
{
    [BsonIgnoreExtraElements]
    public class Events
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Eventname { get; set; }
        public string Username { get; set; }
        public string Userid { get; set; }
        public string Active { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }

        public DateTime DateCreated { get; set; }
        public string Eventaddress { get; set; }
        public Stock[] Stocks { get; set; }

        public Table[] Tables { get; set; }

        public LinkedEvent[] LinkedEvents { get; set; }
        public Waiter[] Waiters { get; set; }
        public string[] SuggestedAddons { get; set; }
    }

}
