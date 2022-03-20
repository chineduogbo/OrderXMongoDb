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
    public class TableController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string baseUrl;
        private readonly IMapper _mapper;
        public TableController(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            baseUrl = _configuration.GetValue<string>("BaseUrl:root");
            _mapper = mapper;
        }
        [HttpGet]

        public async Task<TableDto> GetAll(string EventId)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
            var dblist = dbClient.GetDatabase("orderx").GetCollection<Events>("events").AsQueryable().Where(x => x._id == EventId).FirstOrDefault();
            TableDto dto = new() { _id = dblist._id, eventname = dblist.eventname, Table = dblist.Table == null ? null : dblist.Table };
            return dto;
        }

        [HttpPost]
        public async Task<bool> Create(Table model, string EventId)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
            //var existingwaiter = dbClient.GetDatabase("orderx").GetCollection<Users>("users").AsQueryable().Where(x => x.username == model.WaiterUsername).FirstOrDefault();
            //if (existingwaiter != null)
            //{
                var dblist = dbClient.GetDatabase("orderx").GetCollection<Events>("events").AsQueryable().Where(x => x._id == EventId).FirstOrDefault();
                if (dblist.Table == null)
                {
                    Table[] created = new Table[] { new Table() { Id = Guid.NewGuid().ToString(), Name = model.Name, WaiterUsername = model.WaiterUsername } };
                    var filter = Builders<Events>.Filter.Eq("_id", EventId);
                    var update = Builders<Events>.Update.Set("Table", created);
                    await dbClient.GetDatabase("orderx").GetCollection<Events>("events").UpdateOneAsync(filter, update);
                    return true;
                }
                else
                {
                    var checkexisting = dblist.Table.ToList().Where(x => x.Name == model.Name);
                    if (checkexisting.Count() == 0)
                    {
                       Table[] created = null;
                        List<Table> table = new List<Table>();
                       table.AddRange(dblist.Table.ToList());
                       Table value = new Table() { Id = Guid.NewGuid().ToString(), Name = model.Name, WaiterUsername = model.WaiterUsername };
                       table.Add(value);
                        created = table.ToArray();
                        var filter = Builders<Events>.Filter.Eq("_id", EventId);
                        var update = Builders<Events>.Update.Set("Table", created);
                        await dbClient.GetDatabase("orderx").GetCollection<Events>("events").UpdateOneAsync(filter, update);
                        return true;
                    }
                    else
                    {
                    Table[] created = null;
                    List<Table> table = new List<Table>();
                    table.AddRange(dblist.Table.ToList());
                    Table value = new Table() { Id = Guid.NewGuid().ToString(), Name = model.Name, WaiterUsername = model.WaiterUsername };
                    table.Remove(checkexisting.FirstOrDefault());
                    table.Add(value);
                        created = table.ToArray();
                        var filter = Builders<Events>.Filter.Eq("_id", EventId);
                        var update = Builders<Events>.Update.Set("Table", created);
                        await dbClient.GetDatabase("orderx").GetCollection<Events>("events").UpdateOneAsync(filter, update);
                        return true;
                    }
                }
            
            return false;
        }
        [HttpPut]

        public async Task<bool> Edit(Table model, string EventId)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
            var dblist = dbClient.GetDatabase("orderx").GetCollection<Events>("events").AsQueryable().Where(x => x._id == EventId).FirstOrDefault();
            var checkexisting = dblist.Table.ToList().Where(x => x.Id == model.Id);
            if (checkexisting != null)
            {
                Table[] created = null;
                List<Table> table = new List<Table>();
                table.AddRange(dblist.Table.ToList());
                Table value = new Table() { Id = Guid.NewGuid().ToString(), Name = model.Name, WaiterUsername = model.WaiterUsername };
                table.Remove(checkexisting.FirstOrDefault());
                table.Add(value);
                created = table.ToArray();
                var filter = Builders<Events>.Filter.Eq("_id", EventId);
                var update = Builders<Events>.Update.Set("Table", created);
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
            var checkexisting = dblist.Table.ToList().Where(x => x.Id == id);
            if (checkexisting != null)
            {
                Table[] created = null;
                List<Table> waiter = new List<Table>();
                waiter.AddRange(dblist.Table.ToList());
                waiter.Remove(checkexisting.FirstOrDefault());
                created = waiter.ToArray();
                var filter = Builders<Events>.Filter.Eq("_id", EventId);
                var update = Builders<Events>.Update.Set("Table ", created);
                await dbClient.GetDatabase("orderx").GetCollection<Events>("events").UpdateOneAsync(filter, update);

            }

            return new SuccessDTO() { Id = 0, SuccessMessage = "Deleted Successfully" };
        }
        [HttpPost]
        public async Task<SuccessDTO> AssignWaiterToTable(AssingWaiterDto model)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
            var dblist = dbClient.GetDatabase("orderx").GetCollection<Events>("events").AsQueryable().Where(x => x._id == model.EventId).FirstOrDefault();
            var checkexisting = dblist.Table.ToList();
          
            foreach(var item in model.Tables)
            {
              var existing =  checkexisting.Where(x => x.Name == item.Trim()).FirstOrDefault();
                if(existing != null)
                {
                    var table = new Table() { Id = Guid.NewGuid().ToString(), Name = existing.Name, WaiterUsername = model.UserName };
                    checkexisting.Remove(existing);
                    checkexisting.Add(table);
                }
                else
                {
                    var table = new Table() { Id = Guid.NewGuid().ToString(), Name = item, WaiterUsername = model.UserName };
                    checkexisting.Add(table);
                }
            }

           var created = checkexisting.ToArray();
            var filter = Builders<Events>.Filter.Eq("_id", model.EventId);
            var update = Builders<Events>.Update.Set("Table ", created);
            await dbClient.GetDatabase("orderx").GetCollection<Events>("events").UpdateOneAsync(filter, update);

            return new SuccessDTO() { Id = 0, SuccessMessage = "Assigned Successfully" };
        }
    }
}
