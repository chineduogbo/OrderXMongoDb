using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using OrderXMongoWebApi.Models;
using OrderXMongoWebApi.Models.Dto;
using OrderXMongoWebApi.Services.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OrderXMongoWebApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
   
    public class UsersController : ControllerBase
    {
        //private readonly iCRUDService<Users> _crudservice;
        private readonly IConfiguration _configuration;
        private readonly string baseUrl;
        public UsersController(/*iCRUDService<Users> crudservice*/ IConfiguration configuration)
        {
            //_crudservice = crudservice;
            _configuration = configuration;
            baseUrl = _configuration.GetValue<string>("BaseUrl:root");
        }
        [HttpGet]
        
        public async Task< IEnumerable<Users>> GetAllUsers()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
            var dblist = dbClient.GetDatabase("orderx").GetCollection<Users>("users").AsQueryable();
            //return  await _crudservice.GetAll("Users");
            return dblist;
        }
        [HttpGet]
      
        public async Task<Users> GetUserById(string id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
            var dblist = dbClient.GetDatabase("orderx").GetCollection<Users>("users").AsQueryable().Where(x=>x._id == id).FirstOrDefault();
            //return  await _crudservice.GetAll("Users");
            return dblist;
        }
        [HttpPost]
        public async Task<bool>  CreateUser(Users model)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));

            dbClient.GetDatabase("orderx").GetCollection<Users>("users").InsertOne(model);
            return true;
        }
        [HttpPut]
       
        public JsonResult EditUser(Users model)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
            var filter = Builders<Users>.Filter.Eq("_id", model._id);
            var update = Builders<Users>.Update.Set("FavouriteBrands", model.FavouriteBrands);
            var updatefullname = Builders<Users>.Update.Set("FullName", model.FullName);
            dbClient.GetDatabase("orderx").GetCollection<Users>("users").UpdateOne(filter, update);
            dbClient.GetDatabase("orderx").GetCollection<Users>("users").UpdateOne(filter, updatefullname);

            return new JsonResult("updated successfully");
        }
        [HttpDelete("{id}")]
        [Authorize(Policy = "User")]
        public async Task<SuccessDTO> Delete(string id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
            var filter = Builders<Users>.Filter.Eq("_id", id);
            dbClient.GetDatabase("orderx").GetCollection<Users>("users").DeleteOne(filter);

            return new SuccessDTO() { Id = 0, SuccessMessage = "Deleted Successfully" };
        }
        [HttpPost]
        public async Task<ReturnLoginDto> Login(LoginDto User)
        {
            // TODO: Authenticate Admin with Database
            // If not authenticate return 401 Unauthorized
            // Else continue with below flow
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("orderxconnection"));
            var dblist = dbClient.GetDatabase("orderx").GetCollection<Users>("users").AsQueryable().Where(x => x.username == User.Username && x.password == User.Password).FirstOrDefault();
            if (dblist != null)
            {
                var Claims = new List<Claim>
            {
                new Claim("type", "User"),
            };

                var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SXkSqsKyNUyvGbnHs7ke2NCq8zQzNLW7mPmHbnZZ"));

                var Token = new JwtSecurityToken(
                    baseUrl,
                    baseUrl,
                    Claims,
                    expires: DateTime.Now.AddDays(30.0),
                    signingCredentials: new SigningCredentials(Key, SecurityAlgorithms.HmacSha256)
                );
                return new()
                {
                    Authkey = (new JwtSecurityTokenHandler().WriteToken(Token)).ToString(),
                    Username = dblist.username,
                    FullName = dblist.FullName,
                    UserId = dblist._id
                };
            }
            return null;
        }

    }
}
