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

namespace ApiBase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepository _repository;
     
        public UsersController(IWebHostEnvironment env, IConfiguration appsettings, AppDbContext context, IRepository repository)
        {
            _context = context;
            _repository = repository;
          
        }
        // GET: api/<UsersController>
        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
            Respuesta<object> respuesta = new Respuesta<object>();
            try
            {
                var users = await _repository.SelectAll<Users>();

                foreach (var item in users)
                {
                    if (item.DeletedAt == null)
                    {
                        respuesta.Data.Add(item);
                    }
                }
                respuesta.Ok = 1;
                respuesta.Message = "Success";
            }
            catch (Exception e)
            {
                respuesta.Ok = 0;
                respuesta.Message = e.Message + " " + e.InnerException;
            }
            return Ok(respuesta);
        }

        //GET api/<QuestionsVrsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetUser(int id)
        {
            Respuesta<object> respuesta = new Respuesta<object>();
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(q => q.Id == id);
                if (user != null && user.DeletedAt == null)
                {
                    respuesta.Data.Add(new
                    {
                       user.Id,
                        user.FirstName,
                        user.LastName,
                        user.Email,
                        user.Username,
                        user.Password


                    });
                    respuesta.Ok = 1;
                    respuesta.Message = "User found successfully";
                }
                else
                {
                    respuesta.Ok = 0;
                    respuesta.Message = "User not found";
                    return Ok(respuesta);
                }
            }
            catch (Exception e)
            {
                respuesta.Ok = 0;
                respuesta.Message = e.Message + " " + e.InnerException;
                return Ok(respuesta);
            }
            return Ok(respuesta);
        }

        //GET api/<QuestionsVrsController>/5

        [HttpPost("Create")]
        public async Task<IActionResult> Register(Users userR)
        {
            Respuesta<object> respuesta = new Respuesta<object>();
          
            try
            {
                userR.Email = UsersConfig.CheckGmail(userR.Email);
                Users user = await _context.Users.Where(u => u.Email == userR.Email).FirstOrDefaultAsync();
                if (user != null)
                {
                    respuesta.Ok = 0;
                    respuesta.Message = "User Registered";
                }
                else
                {
                    var newUser = await _repository.CreateAsync(userR);
                    respuesta.Ok = 1;
                    respuesta.Data.Add(newUser);
            
                    respuesta.Message = "User registered Successfully";
                                    
                }
            }
            catch (Exception e)
            {
                respuesta.Ok = 0;
                respuesta.Message = e.Message + " " + e.InnerException;
            }
            return Ok(respuesta);
        }

        // PUT api/<QuestionsController>/5
        [HttpPut("{Id}")]
      
        public async Task<ActionResult> UpdateUser(int Id, Users user)
        {
            Respuesta<object> respuesta = new Respuesta<object>();
            try
            {
                var u = await _context.Users.Where(q => q.Id == Id).FirstOrDefaultAsync();

                if (u != null && u.DeletedAt==null)
                {
                    if (user.FirstName != u.FirstName)
                    { u.FirstName = user.FirstName; }
                    if (user.LastName != u.LastName)
                    { u.LastName = user.LastName; }
                    if (user.Email != u.Email)
                    { u.Email = user.Email; }
                    if (user.Password != u.Password)
                    { u.Password = user.Password; }
                    if (user.Username != u.Username)
                    { u.Username = user.Username; }
                   
                    await _repository.UpdateAsync(u);
                    respuesta.Ok = 1;
                    respuesta.Message = "User updated Successfully";
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
                return Ok(respuesta);
            }
            return Ok(respuesta);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUsers(int id)
        {
            Respuesta<object> respuesta = new Respuesta<object>();
            try
            {
                var user = await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
                if (user != null && user.DeletedAt == null)
                {
                    user.DeletedAt = new DateTime();
                    await _repository.DeleteAsync(user);
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
       
    }
}
