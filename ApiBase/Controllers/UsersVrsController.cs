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
using System.Net.Http;
using System.Text.Json;
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
                respuesta.Message = "Success";
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
                    respuesta.Message = "El usuario ya está registrado";              
                }
                else
                {
                    var usersVr = new UsersVr 
                    {
                        FullName = userR.Fullname,
                        Email = (string)UsersConfig.VerifyGmail(userR.Email),
                        CreatedAt = new TimeZoneChecker(_context, _appsettings).DT(),
                        VerificationCode = token                        
                };
                    await _repository.CreateAsync(usersVr);
                    respuesta.Ok = 1;
                    respuesta.Data.Add(new
                    {                 
                        usersVr.City,
                        usersVr.PostalCode,
                        usersVr.Province,
                        usersVr.State
                    });
                   
                    respuesta.Message = "Success";
                }
            }
            catch (Exception e)
            {
                respuesta.Ok = 0;
                respuesta.Message = e.Message + " " + e.InnerException;
            }
            return Ok(respuesta);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("sepomex")]
        public async Task<IActionResult> PostalCode()
        {
            Respuesta<object> respuesta = new Respuesta<object>();
            //Conexión a la API
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.copomex.com/query/get_estados?token=f9042ad8-6ec0-40c0-bcef-dcbec2ec92f7");
            try
            {
                //Encabezados
                client.DefaultRequestHeaders.Add("Accept-Type", "application/json");

                var response = await client.GetAsync(client.BaseAddress);
                //dynamic json = JsonConvert.DeserializeObject(response);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsByteArrayAsync();

                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

                var aa = JsonSerializer.Deserialize<dynamic>(result, options);

                respuesta.Ok = 1;
                respuesta.Data.Add(new { data = aa });
                respuesta.Message = "Success";
            }
            catch (Exception e)
            {
                respuesta.Ok = 0;
                respuesta.Message = e.Message + " " + e.InnerException;
            }
            return Ok(respuesta);
        }

        //Municipio por Estado
        [HttpGet]
        [AllowAnonymous]
        [Route("sepomex/{state}")]
        public async Task<IActionResult> State(string state)
        {
            Respuesta<object> respuesta = new Respuesta<object>();
        
            //Conexión a la API
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.copomex.com/query/get_municipio_por_estado/"+ state + "?token=f9042ad8-6ec0-40c0-bcef-dcbec2ec92f7");
            try
            {
                //Encabezados
                client.DefaultRequestHeaders.Add("Accept-Type", "application/json");

                var response = await client.GetAsync(client.BaseAddress);
                //dynamic json = JsonConvert.DeserializeObject(response);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsByteArrayAsync();

                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

                var aa = JsonSerializer.Deserialize<dynamic>(result, options);

                respuesta.Ok = 1;
                respuesta.Data.Add(new { data = aa });
                respuesta.Message = "Success";
            }
            catch (Exception e)
            {
                respuesta.Ok = 0;
                respuesta.Message = e.Message + " " + e.InnerException;
            }
            return Ok(respuesta);
        }

        //Colonia por Municipio
        //[HttpGet]
        //[AllowAnonymous]
        //[Route("sepomex/{Province}")]
        //public async Task<IActionResult> Province(string province)
        //{
        //    Respuesta<object> respuesta = new Respuesta<object>();

        //    //Conexión a la API
        //    var client = new HttpClient();
        //    client.BaseAddress = new Uri("https://api.copomex.com/query/get_colonia_por_municipio" + province + "?token=f9042ad8-6ec0-40c0-bcef-dcbec2ec92f7");
        //    try
        //    {
        //        //Encabezados
        //        client.DefaultRequestHeaders.Add("Accept-Type", "application/json");

        //        var response = await client.GetAsync(client.BaseAddress);
        //        //dynamic json = JsonConvert.DeserializeObject(response);
        //        response.EnsureSuccessStatusCode();
        //        var result = await response.Content.ReadAsByteArrayAsync();

        //        var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        //        var aa = JsonSerializer.Deserialize<dynamic>(result, options);

        //        respuesta.Ok = 1;
        //        respuesta.Data.Add(new { data = aa });
        //        respuesta.Message = "Success";
        //    }
        //    catch (Exception e)
        //    {
        //        respuesta.Ok = 0;
        //        respuesta.Message = e.Message + " " + e.InnerException;
        //    }
        //    return Ok(respuesta);
        //}

        [HttpPut("{Id}")]
        public async Task<ActionResult> PutUserVr(int Id, UsersVr userVr)
        {
            Respuesta<object> respuesta = new Respuesta<object>();
            try
            {
                var u = await _context.UsersVrs.Where(u => u.Id == Id).FirstOrDefaultAsync();

                if (u != null && u.DeletedAt == null)
                {
                    if (userVr.FullName != u.FullName)
                    { u.FullName = userVr.FullName; }
                    if (userVr.Email != u.Email)
                    { u.Email = userVr.Email; }

                    u.UpdatedAt = new TimeZoneChecker(_context, _appsettings).DT();
                    await _repository.UpdateAsync(u);
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
                return Ok(respuesta);
            }
            return Ok(respuesta);

        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteUsers18(int Id)
        {
            Respuesta<object> respuesta = new Respuesta<object>();
            try
            {
                var user = await _context.UsersVrs.Where(u => u.Id == Id).FirstOrDefaultAsync();
                if (user != null && user.DeletedAt == null)
                {
                    user.DeletedAt = new TimeZoneChecker(_context, _appsettings).DT();
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

    public class UsersVrRegister
    {
        [Required(ErrorMessage = "Fullname is required")]
        [MaxLength(50)]
        public string Fullname { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [MaxLength(50)]
        public string Email { get; set; }
    }
    //public class UpdateModel
    //{
    //    public string FullName { get; set; }       
    //    [EmailAddress]
    //    public string Email { get; set; }
       
    //}


}
