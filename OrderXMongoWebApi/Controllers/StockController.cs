using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using OrderXMongoWebApi.Models;
using OrderXMongoWebApi.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderXMongoWebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string baseUrl;
        private readonly IMapper _mapper;
        public StockController(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            baseUrl = _configuration.GetValue<string>("BaseUrl:root");
            _mapper = mapper;
        }
        [HttpGet]

        public async Task<StockDto> GetAll(string EventId)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
            var dblist = dbClient.GetDatabase("orderx").GetCollection<Events>("events").AsQueryable().Where(x => x._id == EventId).FirstOrDefault();
            StockDto stockDto =new (){_id = dblist._id,eventname=dblist.eventname,Stock = dblist.Stock ==null?null : dblist.Stock};
            return stockDto;
        }
      
        [HttpPost]
        public async Task<bool> Create(Stock model,string EventId)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
            var dblist = dbClient.GetDatabase("orderx").GetCollection<Events>("events").AsQueryable().Where(x => x._id == EventId).FirstOrDefault();
            if (dblist.Stock == null) 
            {
                Stock[] createdstock = new Stock[] { new Stock() { Id = Guid.NewGuid().ToString(),Category = model.Category,Name = model.Category,Price = model.Price,Quantity = model.Quantity} };
                var filter = Builders<Events>.Filter.Eq("_id", EventId);
                var update = Builders<Events>.Update.Set("Stock", createdstock);
                await dbClient.GetDatabase("orderx").GetCollection<Events>("events").UpdateOneAsync(filter, update);
                return true;
            }
            else
            {
                var checkexistingstock = dblist.Stock.ToList().Where(x => x.Name == model.Name);
                if (checkexistingstock.Count()==0)
                {
                    Stock[] createdstock = null;
                    List<Stock> stocks = new List<Stock>();
                    stocks.AddRange(dblist.Stock.ToList());
                    Stock value = new Stock() { Id = Guid.NewGuid().ToString(), Category = model.Category, Name = model.Category, Price = model.Price, Quantity = model.Quantity };
                    stocks.Add(value);
                    createdstock = stocks.ToArray();
                    var filter = Builders<Events>.Filter.Eq("_id", EventId);
                    var update = Builders<Events>.Update.Set("Stock", createdstock);
                    await dbClient.GetDatabase("orderx").GetCollection<Events>("events").UpdateOneAsync(filter, update);
                    return true;
                }
                else
                {
                    Stock[] createdstock = null;
                    List<Stock> stocks = new List<Stock>();
                    stocks.AddRange(dblist.Stock.ToList());
                    Stock value = new Stock() { Id = Guid.NewGuid().ToString(), Category = model.Category, Name = model.Category, Price = model.Price, Quantity = model.Quantity };
                    stocks.Remove(checkexistingstock.FirstOrDefault());
                    stocks.Add(value);
                    createdstock = stocks.ToArray();
                    var filter = Builders<Events>.Filter.Eq("_id", EventId);
                    var update = Builders<Events>.Update.Set("Stock", createdstock);
                    await dbClient.GetDatabase("orderx").GetCollection<Events>("events").UpdateOneAsync(filter, update);
                    return true;
                }
            }
            return false;
        }
        [HttpPut]

        public async Task<bool> Edit(Stock model, string EventId)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
            var dblist = dbClient.GetDatabase("orderx").GetCollection<Events>("events").AsQueryable().Where(x => x._id == EventId).FirstOrDefault();
            var checkexistingstock = dblist.Stock.ToList().Where(x => x.Id == model.Id);
            if(checkexistingstock != null)
            {
                Stock[] createdstock = null;
                List<Stock> stocks = new List<Stock>();
                stocks.AddRange(dblist.Stock.ToList());
                Stock value = new Stock() { Id = Guid.NewGuid().ToString(), Category = model.Category, Name = model.Category, Price = model.Price, Quantity = model.Quantity };
                stocks.Remove(checkexistingstock.FirstOrDefault());
                stocks.Add(value);
                createdstock = stocks.ToArray();
                var filter = Builders<Events>.Filter.Eq("_id", EventId);
                var update = Builders<Events>.Update.Set("Stock", createdstock);
                await dbClient.GetDatabase("orderx").GetCollection<Events>("events").UpdateOneAsync(filter, update);
                return true;
            }
          return false;
        }
        [HttpDelete("{id}")]
        
        public async Task<SuccessDTO> Delete(string id, string EventId)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
            var dblist = dbClient.GetDatabase("orderx").GetCollection<Events>("events").AsQueryable().Where(x => x._id == EventId).FirstOrDefault();
            var checkexistingstock = dblist.Stock.ToList().Where(x => x.Id == id);
            if (checkexistingstock != null)
            {
                Stock[] createdstock = null;
                List<Stock> stocks = new List<Stock>();
                stocks.AddRange(dblist.Stock.ToList());
                stocks.Remove(checkexistingstock.FirstOrDefault());
                createdstock = stocks.ToArray();
                var filter = Builders<Events>.Filter.Eq("_id", EventId);
                var update = Builders<Events>.Update.Set("Stock", createdstock);
                await dbClient.GetDatabase("orderx").GetCollection<Events>("events").UpdateOneAsync(filter, update);
               
            }
         
            return new SuccessDTO() { Id = 0, SuccessMessage = "Deleted Successfully" };
        }
    }
}
