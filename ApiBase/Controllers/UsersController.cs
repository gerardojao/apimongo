using ApiBase.Models;
using ApiBase.Utils;
using ApiBase.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiBase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepository _repository;
        private readonly IWebHostEnvironment _env;

        private readonly IConfiguration _appsettings;
        public UsersController(IWebHostEnvironment env, IConfiguration appsettings, AppDbContext context, IRepository repository)
        {
            _context = context;
            _repository = repository;
            _appsettings = appsettings;
            _env = env;
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
                //userR.Email = UsersConfig.CheckGmail(userR.Email);
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
        //public ActionResult Delete(int id)
        //{
        //    try
        //    {
        //        Users users = _context.Users.FirstOrDefault(u => u.Id == id);
        //        if (users != null)
        //        {
        //             users.DeletedAt = new TimeZoneChecker(_context, _appsettings).DT();
        //            _context.Update(users);
        //            _context.SaveChanges();
        //            return Ok();
        //        }
        //        else
        //        {
        //            return BadRequest();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpGet("verify/{email}")]
        //[AllowAnonymous]
        //// Para desactivar app
        //// [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "CREATOR")]
        //public async Task<IActionResult> ConfirmUser(string email, string token)
        //{
        //    Respuesta<object> respuesta = new Respuesta<object>();
        //    try
        //    {
        //        var auxemail = UsersConfig.CheckGmail(email);
        //        var user = await _context.Users.Where(c => c.Email == auxemail).FirstOrDefaultAsync();
        //        if (user != null)
        //        {
        //            if (user.EmailConfirmed == false)
        //            {

        //                    var userConfirmed = user.EmailConfirmed;
        //                    user.EmailConfirmed = true;
        //                    await _repository.VerifyUser(user);

        //                    respuesta.Ok = 1;
        //                    respuesta.Message = " Email verified";

        //            }
        //            else
        //            {
        //                respuesta.Ok = 0;
        //                respuesta.Message = "Email already confirmed";
        //            }
        //        }
        //        else
        //        {
        //            respuesta.Ok = 0;
        //            respuesta.Message = "Email not found";
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        respuesta.Ok = 0;
        //        respuesta.Message = e.Message + " " + e.InnerException;
        //    }
        //    return Ok(respuesta);
        //}

        //private async Task<Boolean> SendEmail(UsersVr user, string fileName, string asunto)
        //{
        //    var baseUrl = (Request.IsHttps) ? "https://" + this.Request.Host : "http://" + this.Request.Host;
        //    Dictionary<string, string> tags = new Dictionary<string, string>(); // Create tags for email template.
        //    tags.Add("[[DOMAIN]]", "Dominio");
        //    tags.Add("[[BASEURL]]", baseUrl);
        //    tags.Add("[[CODE]]", user.VerificationCode);
        //    tags.Add("[[RETURN_LINK]]", this._appsettings["SendGrid:APP_HOST"]);
        //    var webRoot = _env.WebRootPath;
        //    var pathToFile = _env.WebRootPath
        //        + Path.DirectorySeparatorChar.ToString()
        //        + "template"
        //        + Path.DirectorySeparatorChar.ToString()
        //        + fileName;

        //    string htmlBody = HTMLTemplateHelper.parseFromFile(pathToFile, tags);

        //    var response = await EmailMessage.SendEmail(
        //        this._appsettings["SendGrid:API_KEY"],
        //        this._appsettings["SendGrid:Email"],
        //        this._appsettings["SendGrid:Name"],
        //        user.Email, asunto, htmlBody);

        //    return response;
        //}

        //[HttpPost("RecoverCode")]
        //[AllowAnonymous]
        //// Para desactivar app
        //// [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "CREATOR")]
        //public async Task<IActionResult> RecoverCode(string email)
        //{
        //    Respuesta<object> respuesta = new Respuesta<object>();
        //    try
        //    {
        //        var user = await _context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();

        //        var response = await SendEmail(user, "VerificationCodeTemplate.html", "Recuperación del código de verificación");
        //        if (response)
        //        {
        //            respuesta.Ok = 1;           
        //            respuesta.Message = "Success";
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        respuesta.Ok = 0;

        //        respuesta.Message = e.Message + " " + e.InnerException;

        //    }
        //    return Ok(respuesta);
        //}
    }
}
