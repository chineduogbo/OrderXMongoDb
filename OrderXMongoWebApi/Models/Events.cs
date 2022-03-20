using MongoDB.Bson;
using System;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace OrderXMongoWebApi.Models
{
    [BsonIgnoreExtraElements]
    public class Events
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? _id { get; set; }
        public string eventname { get; set; }
        public string username { get; set; }
        public string userid { get; set; }
        public bool active { get; set; }
        public decimal lat { get; set; }
        public decimal lng { get; set; }

        public DateTime DateCreated { get; set; }
        public string eventaddress { get; set; }
        
        public Stock[] Stock { get; set; }

        public Table[] Table { get; set; }

        public LinkedEvent[] LinkedEvent { get; set; }
        public Waiter[] Waiter { get; set; }
        public string[] SuggestedAddons { get; set; }
    }

}
