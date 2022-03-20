//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using MongoDB.Driver;
//using OrderXMongoWebApi.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace OrderXMongoWebApi.Controllers
//{
//    [ApiController]
//    [Route("[controller]/[action]")]
//    public class WeatherForecastController : ControllerBase
//    {
//        private readonly IConfiguration _configuration;
       
//        private static readonly string[] Summaries = new[]
//        {
//            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//        };

//        private readonly ILogger<WeatherForecastController> _logger;

//        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration)
//        {
//            _logger = logger;
//            _configuration = configuration;
//        }

//        [HttpGet]
//        public IEnumerable<WeatherForecast> Get()
//        {
//            var rng = new Random();
//            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
//            {
//                Date = DateTime.Now.AddDays(index),
//                TemperatureC = rng.Next(-20, 55),
//                Summary = Summaries[rng.Next(Summaries.Length)]
//            })
//            .ToArray();
//        }
//        [HttpGet]
//        public IEnumerable<Users> GetAllValues()
//        {
//            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
//            var dblist = dbClient.GetDatabase("orderx").GetCollection<Users>("users").AsQueryable();
//            return dblist;
//        }
//        [HttpPost]
//        public JsonResult CreateUser(Users model)
//        {
//            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));

//            dbClient.GetDatabase("orderx").GetCollection<Users>("users").InsertOne(model);
//            return new JsonResult("added successfully");
//        }
//        [HttpPut]
//        public JsonResult EditUser(Users model)
//        {
//            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
//            var filter = Builders<Users>.Filter.Eq("_id", model._id);
//            var update = Builders<Users>.Update.Set("FavouriteBrands", model.FavouriteBrands);
//            var updatefullname = Builders<Users>.Update.Set("FullName", model.FullName);
//            dbClient.GetDatabase("orderx").GetCollection<Users>("users").UpdateOne(filter,update);
//            dbClient.GetDatabase("orderx").GetCollection<Users>("users").UpdateOne(filter, updatefullname);

//            return new JsonResult("updated successfully");
//        }
//        [HttpDelete("{id}")]
//        public JsonResult Delete(string id)
//        {
//            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
//            var filter = Builders<Users>.Filter.Eq("_id", id);
//              dbClient.GetDatabase("orderx").GetCollection<Users>("users").DeleteOne(filter);

//            return new JsonResult("deleted successfully");
//        }
//    }
//}
