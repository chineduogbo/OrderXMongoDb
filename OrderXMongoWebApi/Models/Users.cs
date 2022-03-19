using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderXMongoWebApi.Models
{
    [BsonIgnoreExtraElements]
    public class Users
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? _id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public bool active { get; set; }
        public DateTime? lastlogin { get; set; }

        public string? FullName { get; set; }

        public int? drinklimit { get; set; }
      
        public IList<string>? FavouriteBrands { get; set; }
       
        public string? PhoneNumber { get; set; }
    }

}
