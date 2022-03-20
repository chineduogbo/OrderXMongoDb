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
    public class EventController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string baseUrl;
        private readonly IMapper _mapper;
        public EventController(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            baseUrl = _configuration.GetValue<string>("BaseUrl:root");
            _mapper = mapper;
        }
        [HttpGet]

        public async Task<IEnumerable<EventDto>> GetAllEvents(string userId)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
            var dblist =  dbClient.GetDatabase("orderx").GetCollection<Events>("events").AsQueryable().Where(x=>x.userid==userId);
            return _mapper.Map<IEnumerable<EventDto>>(dblist.ToList());
        }
        [HttpGet]

        public async Task<EventDto> GetEventsById(string id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
            var dblist = dbClient.GetDatabase("orderx").GetCollection<Events>("events").AsQueryable().Where(x => x._id == id).FirstOrDefault();
            //return  await _crudservice.GetAll("Users");
            return _mapper.Map<EventDto>(dblist);
        }
        [HttpPost]
        public async Task<bool> CreateEvents(CreateEventDto model)
        {
            var createevent = _mapper.Map<Events>(model);
            createevent.active = true;
            createevent.DateCreated = DateTime.Now;
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));

           await dbClient.GetDatabase("orderx").GetCollection<Events>("events").InsertOneAsync(createevent);
            return true;
        }
        [HttpPut]

        public async Task<bool> EditEvents(EventDto model)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
            var filter = Builders<Events>.Filter.Eq("_id", model._id);
            var update = Builders<Events>.Update.Set("eventaddress", model.eventaddress);
            var updatefullname = Builders<Events>.Update.Set("eventname", model.eventname);
          await  dbClient.GetDatabase("orderx").GetCollection<Events>("events").UpdateOneAsync(filter, update);
          await  dbClient.GetDatabase("orderx").GetCollection<Events>("events").UpdateOneAsync(filter, updatefullname);

            return true;
        }
        [HttpDelete("{id}")]
        [Authorize(Policy = "User")]
        public async Task<SuccessDTO> Delete(string id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
            var filter = Builders<Events>.Filter.Eq("_id", id);
           await dbClient.GetDatabase("orderx").GetCollection<Events>("events").DeleteOneAsync(filter);

            return new SuccessDTO() { Id = 0, SuccessMessage = "Deleted Successfully" };
        }
    }
}
