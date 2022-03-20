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
    public class WaiterController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string baseUrl;
        private readonly IMapper _mapper;
        public WaiterController(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            baseUrl = _configuration.GetValue<string>("BaseUrl:root");
            _mapper = mapper;
        }
        [HttpGet]

        public async Task<WaiterDto> GetAll(string EventId)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
            var dblist = dbClient.GetDatabase("orderx").GetCollection<Events>("events").AsQueryable().Where(x => x._id == EventId).FirstOrDefault();
            WaiterDto waterdto = new() { _id = dblist._id, eventname = dblist.eventname, Waiter = dblist.Waiter == null ? null : dblist.Waiter };
            return waterdto;
        }

        [HttpPost]
        public async Task<bool> Create(Waiter model, string EventId)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
            var existingwaiter = dbClient.GetDatabase("orderx").GetCollection<Users>("users").AsQueryable().Where(x => x.username == model.WaiterUsername).FirstOrDefault();
            if (existingwaiter != null)
            {
                var dblist = dbClient.GetDatabase("orderx").GetCollection<Events>("events").AsQueryable().Where(x => x._id == EventId).FirstOrDefault();
                if (dblist.Waiter == null)
                {
                    Waiter[] created = new Waiter[] { new Waiter() { Id = Guid.NewGuid().ToString(), Name = model.Name, WaiterUsername = model.WaiterUsername } };
                    var filter = Builders<Events>.Filter.Eq("_id", EventId);
                    var update = Builders<Events>.Update.Set("Waiter", created);
                    await dbClient.GetDatabase("orderx").GetCollection<Events>("events").UpdateOneAsync(filter, update);
                    return true;
                }
                else
                {
                    var checkexisting = dblist.Waiter.ToList().Where(x => x.Name == model.Name);
                    if (checkexisting.Count() == 0)
                    {
                        Waiter[] created = null;
                        List<Waiter> waiter = new List<Waiter>();
                        waiter.AddRange(dblist.Waiter.ToList());
                        Waiter value = new Waiter() { Id = Guid.NewGuid().ToString(), Name = model.Name, WaiterUsername = model.WaiterUsername };
                        waiter.Add(value);
                        created = waiter.ToArray();
                        var filter = Builders<Events>.Filter.Eq("_id", EventId);
                        var update = Builders<Events>.Update.Set("Waiter", created);
                        await dbClient.GetDatabase("orderx").GetCollection<Events>("events").UpdateOneAsync(filter, update);
                        return true;
                    }
                    else
                    {
                        Waiter[] created = null;
                        List<Waiter> waiter = new List<Waiter>();
                        waiter.AddRange(dblist.Waiter.ToList());
                        Waiter value = new Waiter() { Id = Guid.NewGuid().ToString(), Name = model.Name, WaiterUsername = model.WaiterUsername };
                        waiter.Remove(checkexisting.FirstOrDefault());
                        waiter.Add(value);
                        created = waiter.ToArray();
                        var filter = Builders<Events>.Filter.Eq("_id", EventId);
                        var update = Builders<Events>.Update.Set("Waiter", created);
                        await dbClient.GetDatabase("orderx").GetCollection<Events>("events").UpdateOneAsync(filter, update);
                        return true;
                    }
                }
            }
            return false;
        }
        [HttpPut]

        public async Task<bool> Edit(Waiter model, string EventId)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
            var dblist = dbClient.GetDatabase("orderx").GetCollection<Events>("events").AsQueryable().Where(x => x._id == EventId).FirstOrDefault();
            var checkexisting = dblist.Waiter.ToList().Where(x => x.Id == model.Id);
            if (checkexisting != null)
            {
                Waiter[] created = null;
                List<Waiter> waiter = new List<Waiter>();
                waiter.AddRange(dblist.Waiter.ToList());
                Waiter value = new Waiter() { Id = Guid.NewGuid().ToString(), Name = model.Name, WaiterUsername = model.WaiterUsername };
                waiter.Remove(checkexisting.FirstOrDefault());
                waiter.Add(value);
                created = waiter.ToArray();
                var filter = Builders<Events>.Filter.Eq("_id", EventId);
                var update = Builders<Events>.Update.Set("Waiter", created);
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
            var checkexisting = dblist.Waiter.ToList().Where(x => x.Id == id);
            if (checkexisting != null)
            {
                Waiter[] created = null;
                List<Waiter> waiter = new List<Waiter>();
                waiter.AddRange(dblist.Waiter.ToList());
                waiter.Remove(checkexisting.FirstOrDefault());
                created = waiter.ToArray();
                var filter = Builders<Events>.Filter.Eq("_id", EventId);
                var update = Builders<Events>.Update.Set("Waiter", created);
                await dbClient.GetDatabase("orderx").GetCollection<Events>("events").UpdateOneAsync(filter, update);

            }

            return new SuccessDTO() { Id = 0, SuccessMessage = "Deleted Successfully" };
        }
    }
}
