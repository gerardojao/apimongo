using AmstelAPI.Models;
using ApiBase.Data;
using ApiBase.Models;
using ApiBase.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiBase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersVrsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepository _repository;
        private readonly IConfiguration _appsettings;
        public UsersVrsController(IConfiguration appsettings, AppDbContext context, IRepository repository)
        {
            _context = context;
            _repository = repository;
            _appsettings = appsettings;
        }
        // GET: api/<UsersVrsController>
        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
            Respuesta<object> respuesta = new Respuesta<object>();
            try
            {
                var users = await _repository.SelectAll<UsersVr>();
                foreach (var item in users)
                {
                    if (item.DeletedAt == null)
                    {
                        respuesta.Data.Add(item);
                    }
                }
                respuesta.Ok = 1;
                respuesta.Message = "Succes";
            }
            catch (Exception e)
            {
                respuesta.Ok = 0;
                respuesta.Message = e.Message + " " + e.InnerException;
            }
            return Ok(respuesta);
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult> GetUsers18ById(int Id)
        {
            Respuesta<object> respuesta = new Respuesta<object>();
            try
            {
                var user = await _context.UsersVrs.Where(u => u.Id == Id).FirstOrDefaultAsync();
                if (user != null && user.DeletedAt == null)
                {
                    respuesta.Data.Add(user);
                    respuesta.Ok = 1;
                    respuesta.Message = "Success";
                }
                else
                {
                    respuesta.Ok = 0;
                    respuesta.Message = "User not found";
                }
            }
            catch (Exception e)
            {
                respuesta.Ok = 0;
                respuesta.Message = e.Message + " " + e.InnerException;
            }
            return Ok(respuesta);

        }

        // POST api/<UsersVrsController>
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UsersVrRegister userR)
        {
            Respuesta<object> respuesta = new Respuesta<object>();
            String token = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8);

            try
            {
               
                var user = await _context.UsersVrs.Where(u => u.Email == userR.Email).FirstOrDefaultAsync();
                if (user != null)
                {
                    respuesta.Ok = 0;
                    respuesta.Message = "El usuario ya respondió";
                   
                }
                else
                {
                    var usersVrs = new UsersVr()
                    { 
                        FullName = userR.Fullname,
                        Email = userR.Email,                        
                        VerificationCode = token ,
                        CreatedAt = new TimeZoneChecker(_context, _appsettings).DT()
                    };
                  

                    var newUser = await _repository.CreateAsync(usersVrs);

                    respuesta.Data.Add(newUser);

                    respuesta.Ok = 1;
                    respuesta.Message = "Sucess";
                }
            }
            catch (Exception e)
            {
                respuesta.Ok = 0;
                respuesta.Message = e.Message + " " + e.InnerException;
            }
            return Ok(respuesta);
        }

        //// PUT api/<UsersVrsController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<UsersVrsController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }

    public class UsersVrRegister
    {
        [Required(ErrorMessage = "Fullname is required")]
        [MaxLength(80)]
        public string Fullname { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [MaxLength(50)]
        public string Email { get; set; }
    }

   
}
