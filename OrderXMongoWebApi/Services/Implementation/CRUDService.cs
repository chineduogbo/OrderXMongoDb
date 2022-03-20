

using AutoMapper;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using OrderXMongoWebApi.Models;
using OrderXMongoWebApi.Models.Dto;
using OrderXMongoWebApi.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderXMongoWebApi.Services.Implementation
{
    public class CRUDService<T> : iCRUDService<T> where T : class
    {
        private readonly MongoClient dbClient;
        private readonly IConfiguration _configuration;

        public CRUDService(IConfiguration configuration)
        {
            _configuration = configuration;
            string connectid = _configuration.GetConnectionString("orderxconnection");
            dbClient = new MongoClient(connectid);
        }

        public async Task<bool> Create(T model, string ClassName)
        {
            await dbClient.GetDatabase("orderx").GetCollection<T>(ClassName).InsertOneAsync(model);
            return true;
        }

        public async Task<SuccessDTO> Delete(string Id, string ClassName)
        {
            var filter = Builders<T>.Filter.Eq("_id", Id);
            await dbClient.GetDatabase("orderx").GetCollection<T>(ClassName).DeleteOneAsync(filter);
            return (new SuccessDTO() { Id = 0, SuccessMessage = "Deleted Successfully" });
        }

        //public async Task<SuccessDTO> Edit(T model, string ClassName)
        //{
        //    MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
        //    var filter = Builders<Users>.Filter.Eq("_id", model._id);
        //    var update = Builders<Users>.Update.Set("FavouriteBrands", model.FavouriteBrands);
        //    var updatefullname = Builders<Users>.Update.Set("FullName", model.FullName);
        //    dbClient.GetDatabase("orderx").GetCollection<Users>("users").UpdateOne(filter, update);
        //    dbClient.GetDatabase("orderx").GetCollection<Users>("users").UpdateOne(filter, updatefullname);
        //    return (new SuccessDTO() { Id = 1, SuccessMessage = "Edited Successfully" });
        //}

        public async Task<IEnumerable<T>> GetAll(string ClassName)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
           // var dblist = dbClient.GetDatabase("orderx").GetCollection<Users>("users").AsQueryable();
            var dblist =  dbClient.GetDatabase("orderx").GetCollection<T>(ClassName).AsQueryable();
            return await dblist.ToListAsync();
        }

        public async Task<T> GetById(string Id, string ClassName)
        {
            var filter = Builders<T>.Filter.Eq("_id", Id);
            var dblist = await dbClient.GetDatabase("orderx").GetCollection<T>(ClassName).FindAsync(filter);
            return dblist.FirstOrDefault();
        }

      

     

    }
}
