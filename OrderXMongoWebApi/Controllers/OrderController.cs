using AutoMapper;
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
    public class OrderController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string baseUrl;
        private readonly IMapper _mapper;
        public OrderController(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            baseUrl = _configuration.GetValue<string>("BaseUrl:root");
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ScanTableDto> ScanBarcode(string EventId, string TableId)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
            var dblist = dbClient.GetDatabase("orderx").GetCollection<Events>("events").AsQueryable().Where(x => x._id == EventId).FirstOrDefault();
            var table = dblist.Table.Where(x => x.Id == TableId).FirstOrDefault();
            ScanTableDto dto = new() {  eventname = dblist.eventname,WaiterUserName = table.WaiterUsername,Eventid = dblist._id,Tableid = table.Id,Tablename=table.Name};
            return dto;
        }
       

        [HttpPost]
        public async Task<bool> OpenOrder(CreateOrderDto model)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
            var createmodel = _mapper.Map<Order>(model);
            createmodel.DateOpened = DateTime.Now;
            createmodel.Active = true;
            dbClient.GetDatabase("orderx").GetCollection<Order>("orders").InsertOne(createmodel);
            return true;
        }
        [HttpGet]
        public async Task<OrderDto> GetOpenOrders(string UserId)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
            var dblist = dbClient.GetDatabase("orderx").GetCollection<Order>("orders").AsQueryable().Where(x => x.UserId == UserId && x.Active == true ).FirstOrDefault();
            var dto = _mapper.Map<OrderDto>(dblist);
            return dto;
        }
        //[HttpPut]

        //public async Task<bool> Edit(Stock model, string EventId)
        //{
        //    MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
        //    var dblist = dbClient.GetDatabase("orderx").GetCollection<Events>("events").AsQueryable().Where(x => x._id == EventId).FirstOrDefault();
        //    var checkexistingstock = dblist.Stock.ToList().Where(x => x.Id == model.Id);
        //    if (checkexistingstock != null)
        //    {
        //        Stock[] createdstock = null;
        //        List<Stock> stocks = new List<Stock>();
        //        stocks.AddRange(dblist.Stock.ToList());
        //        Stock value = new Stock() { Id = Guid.NewGuid().ToString(), Category = model.Category, Name = model.Category, Price = model.Price, Quantity = model.Quantity };
        //        stocks.Remove(checkexistingstock.FirstOrDefault());
        //        stocks.Add(value);
        //        createdstock = stocks.ToArray();
        //        var filter = Builders<Events>.Filter.Eq("_id", EventId);
        //        var update = Builders<Events>.Update.Set("Stock", createdstock);
        //        await dbClient.GetDatabase("orderx").GetCollection<Events>("events").UpdateOneAsync(filter, update);
        //        return true;
        //    }
        //    return false;
        //}
        
    }
}
