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
    [Route("api/[controller]")]
    [ApiController]
    public class LinkedEventController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly string baseUrl;
        private readonly IMapper _mapper;
        public LinkedEventController(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            baseUrl = _configuration.GetValue<string>("BaseUrl:root");
            _mapper = mapper;
        }
        [HttpGet]

        public async Task<LinkedEventDto> GetAll(string EventId)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
            var dblist = dbClient.GetDatabase("orderx").GetCollection<Events>("events").AsQueryable().Where(x => x._id == EventId).FirstOrDefault();
            LinkedEventDto dto = new() { _id = dblist._id, eventname = dblist.eventname, LinkedEvent = dblist.LinkedEvent == null ? null : dblist.LinkedEvent };
            return dto;
        }

        [HttpPost]
        public async Task<bool> Create(LinkedEvent model, string EventId)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
            var existingwaiter = dbClient.GetDatabase("orderx").GetCollection<Events>("events").AsQueryable().Where(x => x.username == model.EventUserName && x.eventname == model.EventName).FirstOrDefault();
            if (existingwaiter != null)
            {
                var dblist = dbClient.GetDatabase("orderx").GetCollection<Events>("events").AsQueryable().Where(x => x._id == EventId).FirstOrDefault();
                if (dblist.Waiter == null)
                {
                    LinkedEvent[] created = new LinkedEvent[] { new LinkedEvent() { Id = Guid.NewGuid().ToString(), EventName= model.EventName,EventUserName = model.EventUserName} };
                    var filter = Builders<Events>.Filter.Eq("_id", EventId);
                    var update = Builders<Events>.Update.Set("LinkedEvent", created);
                    await dbClient.GetDatabase("orderx").GetCollection<Events>("events").UpdateOneAsync(filter, update);
                    return true;
                }
                else
                {
                    var checkexisting = dblist.LinkedEvent.ToList().Where(x => x.EventUserName == model.EventUserName && x.EventName == model.EventName);
                    if (checkexisting.Count() == 0)
                    {
                        LinkedEvent[] created = null;
                        List<LinkedEvent> waiter = new List<LinkedEvent>();
                        waiter.AddRange(dblist.LinkedEvent.ToList());
                        LinkedEvent value = new LinkedEvent() { Id = Guid.NewGuid().ToString(), EventName = model.EventName, EventUserName = model.EventUserName };
                        waiter.Add(value);
                        created = waiter.ToArray();
                        var filter = Builders<Events>.Filter.Eq("_id", EventId);
                        var update = Builders<Events>.Update.Set("LinkedEvent", created);
                        await dbClient.GetDatabase("orderx").GetCollection<Events>("events").UpdateOneAsync(filter, update);
                        return true;
                    }
                    else
                    {
                        LinkedEvent[] created = null;
                        List<LinkedEvent> waiter = new List<LinkedEvent>();
                        waiter.AddRange(dblist.LinkedEvent.ToList());
                        LinkedEvent value = new LinkedEvent() { Id = Guid.NewGuid().ToString(), EventName = model.EventName, EventUserName = model.EventUserName };
                        waiter.Remove(checkexisting.FirstOrDefault());
                        waiter.Add(value);
                        created = waiter.ToArray();
                        var filter = Builders<Events>.Filter.Eq("_id", EventId);
                        var update = Builders<Events>.Update.Set("LinkedEvent", created);
                        await dbClient.GetDatabase("orderx").GetCollection<Events>("events").UpdateOneAsync(filter, update);
                        return true;
                    }
                }
            }
            return false;
        }
        [HttpPut]

        public async Task<bool> Edit(LinkedEvent model, string EventId)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
            var dblist = dbClient.GetDatabase("orderx").GetCollection<Events>("events").AsQueryable().Where(x => x._id == EventId).FirstOrDefault();
            var checkexisting = dblist.LinkedEvent.ToList().Where(x => x.Id == model.Id);
            if (checkexisting != null)
            {
                LinkedEvent[] created = null;
                List<LinkedEvent> waiter = new List<LinkedEvent>();
                waiter.AddRange(dblist.LinkedEvent.ToList());
                LinkedEvent value = new LinkedEvent() { Id = Guid.NewGuid().ToString(), EventName = model.EventName, EventUserName = model.EventUserName };
                waiter.Remove(checkexisting.FirstOrDefault());
                waiter.Add(value);
                created = waiter.ToArray();
                var filter = Builders<Events>.Filter.Eq("_id", EventId);
                var update = Builders<Events>.Update.Set("LinkedEvent", created);
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
            var checkexisting = dblist.LinkedEvent.ToList().Where(x => x.Id == id);
            if (checkexisting != null)
            {
                LinkedEvent[] created = null;
                List<LinkedEvent> waiter = new List<LinkedEvent>();
                waiter.AddRange(dblist.LinkedEvent.ToList());
                waiter.Remove(checkexisting.FirstOrDefault());
                created = waiter.ToArray();
                var filter = Builders<Events>.Filter.Eq("_id", EventId);
                var update = Builders<Events>.Update.Set("LinkedEvent", created);
                await dbClient.GetDatabase("orderx").GetCollection<Events>("events").UpdateOneAsync(filter, update);

            }

            return new SuccessDTO() { Id = 0, SuccessMessage = "Deleted Successfully" };
        }
    }
}
