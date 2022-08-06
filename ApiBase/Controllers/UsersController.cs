using ApiBase.Models;
using ApiBase.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using ApiBase.Utils;
using MongoDB.Driver;


namespace ApiBase.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
       

        public UsersController( IConfiguration configuration)
        {
            _configuration = configuration;           
          
        }
       

        [HttpGet]
        public JsonResult Get()
        {

         
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("dBConnection"));
            var dbList = dbClient.GetDatabase("newproject").GetCollection<Users>("users").AsQueryable();

            return new JsonResult(dbList);
        }

        [HttpPost]
        public JsonResult Post(Users us)
        {

            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("dBConnection"));
            int lastUserId = dbClient.GetDatabase("newproject").GetCollection<Users>("users").AsQueryable().Count();
            us.UserId = lastUserId + 1;

            dbClient.GetDatabase("newproject").GetCollection<Users>("users").InsertOne(us);

            return new JsonResult("Added Succesfully");
        }


        [HttpPut]
        public JsonResult Put(int Id, Users us)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("dBConnection"));

            var filter = Builders<Users>.Filter.Eq("UserId", Id);

            var update = Builders<Users>.Update.Set("UserId", us.UserId)
                                               .Set("UserName", us.UserName)
                                                .Set("Name", us.Name)
                                                .Set("LastName", us.LastName)
                                                .Set("Email", us.Email);
                                               

            dbClient.GetDatabase("newproject").GetCollection<Users>("users").UpdateOne(filter, update);

            return new JsonResult("Updated Succesfully");
        }


        [HttpDelete]
        public JsonResult Delete(int Id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("dBConnection"));

            var filter = Builders<Users>.Filter.Eq("UserId", Id);
        

            dbClient.GetDatabase("newproject").GetCollection<Users>("users").DeleteOne(filter);

            return new JsonResult("Deleted Succesfully");
        }


    }
}
