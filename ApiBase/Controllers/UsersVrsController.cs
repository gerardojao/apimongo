using AmstelAPI.Models;
using AmstelAPI.Utils;
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

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UsersVr userR)
        {
            Respuesta<object> respuesta = new Respuesta<object>();
            string token = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8);
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
                    userR.CreatedAt = new TimeZoneChecker(_context, _appsettings).DT();
                    userR.FullName = userR.FirstName + " " + userR.LastName;

                  
                    var userId = await _repository.CreateAsync(user);
                    respuesta.Ok = 1;
                    respuesta.Data.Add(new
                    {
                        Id = userId
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
        [Route("StateList")]
        public async Task<IActionResult> GetState()
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
        [Route("MunicipalyList/{state}")]
        public async Task<IActionResult> GetMunicipalyByState(string state)
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
        [HttpGet]
        [AllowAnonymous]
        [Route("ColonyByMunicipaly/{Province}")]
        public async Task<IActionResult> GetColonyByMunicipaly(string municipaly)
        {
            Respuesta<object> respuesta = new Respuesta<object>();

            //Conexión a la API
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.copomex.com/query/get_colonia_por_municipio" + municipaly + "?token=f9042ad8-6ec0-40c0-bcef-dcbec2ec92f7");
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



        //[HttpPost("code")]
        //[AllowAnonymous]
        //public async Task<IActionResult> resendCode(UsersVr userR)
        //{
        //    Respuesta<object> respuesta = new Respuesta<object>();
        //    var _code = awa
        //    try
        //    {
        //        var user = await _context.UsersVrs.Where(u => u.Email == userR.Email).FirstOrDefaultAsync();
        //        if (user != null)
        //        {
        //            respuesta.Ok = 0;
        //            respuesta.Message = "El usuario ya está registrado";
        //        }
        //        else
        //        {
        //            userR.CreatedAt = new TimeZoneChecker(_context, _appsettings).DT();
        //            userR.FullName = userR.FirstName + " " + userR.LastName;


        //            var userId = await _repository.CreateAsync(user);
        //            respuesta.Ok = 1;
        //            respuesta.Data.Add(new
        //            {
        //                Id = userId
        //            });

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

        [HttpGet("verify/{email}")]
        [AllowAnonymous]
        // Para desactivar app
        // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "CREATOR")]
        public async Task<IActionResult> ConfirmUser(string email, string token)
        {
            Respuesta<object> respuesta = new Respuesta<object>();
            try
            {
                var auxemail = UsersConfig.CheckGmail(email);
                var user = await _context.UsersVrs.Where(c => c.Email == auxemail).FirstOrDefaultAsync();
                if (user != null)
                {
                    if (user.VerificationCode == token)
                    {
                        var userConfirmed = user.EmailConfirmed;
                        user.EmailConfirmed = true;
                        var result = await _repository.VerifyUser(user);

                        respuesta.Ok = 1;
                        respuesta.Message = " Email verified";
                    }
                    else
                    {
                        respuesta.Ok = 0;
                        respuesta.Message = "Missing Token";
                    }
                }
                else
                {
                    respuesta.Ok = 0;
                    respuesta.Message = "Email not found";
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
