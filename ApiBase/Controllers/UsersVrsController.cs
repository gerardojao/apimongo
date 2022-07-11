using AmstelAPI.Models;
using AmstelAPI.Utils;
using ApiBase.Data;
using ApiBase.Models;
using ApiBase.Utils;
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
    public class UsersVrsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepository _repository;
        private readonly IWebHostEnvironment _env;

        private readonly IConfiguration _appsettings;
        public UsersVrsController(IWebHostEnvironment env, IConfiguration appsettings, AppDbContext context, IRepository repository)
        {
            _context = context;
            _repository = repository;
            _appsettings = appsettings;
            _env = env;
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

        //GET api/<QuestionsVrsController>/5
        [HttpGet("GetCode/{email}")]
        public async Task<ActionResult> GetCode(string email)
        {
            Respuesta<object> respuesta = new Respuesta<object>();
            try
            {
                var user = await _context.UsersVrs.FirstOrDefaultAsync(u => u.Email == email);
                if (user != null && user.DeletedAt == null)
                {
                    respuesta.Data.Add(new
                    {
                        user.VerificationCode
                     });
                    respuesta.Ok = 1;
                    respuesta.Message = "Success";
                }
                else
                {
                    respuesta.Ok = 0;
                    respuesta.Message = "user not found";
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
                    userR.VerificationCode = token;

                    var userId = await _repository.CreateAsync(userR);
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
        [Route("ColonyByMunicipaly/{municipaly}")]
        public async Task<IActionResult> GetColonyByMunicipaly(string municipaly)
        {
            Respuesta<object> respuesta = new Respuesta<object>();

            //Conexión a la API
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.copomex.com/query/get_colonia_por_municipio/" + municipaly + "?token=f9042ad8-6ec0-40c0-bcef-dcbec2ec92f7");
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

        private async Task<Boolean> SendEmail(UsersVr user, string code, string fileName, string asunto)
        {
            var baseUrl = (Request.IsHttps) ? "https://" + this.Request.Host : "http://" + this.Request.Host;
            Dictionary<string, string> tags = new Dictionary<string, string>(); // Create tags for email template.
            tags.Add("[[DOMAIN]]", "Dominio");
            tags.Add("[[BASEURL]]", baseUrl);
            tags.Add("[[CODE]]", code);
            tags.Add("[[RETURN_LINK]]", this._appsettings["SendGrid:APP_HOST"]);
            var webRoot = _env.WebRootPath;
            var pathToFile = _env.WebRootPath
                + Path.DirectorySeparatorChar.ToString()
                + "template"
                + Path.DirectorySeparatorChar.ToString()
                + fileName;

            string htmlBody = HTMLTemplateHelper.parseFromFile(pathToFile, tags);
           
            var response = await EmailMessage.SendEmail(
                this._appsettings["SendGrid:API_KEY"],
                this._appsettings["SendGrid:Email"],
                this._appsettings["SendGrid:Name"],
                user.Email, asunto, htmlBody);
          
            return response;
        }

        [HttpPost("SendCode")]
        [AllowAnonymous]
        // Para desactivar app
        // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "CREATOR")]
        public async Task<IActionResult> SendCode(RecoveryForm userForm)
        {
            Respuesta<object> respuesta = new Respuesta<object>();
            try
            {
                var user = await _context.UsersVrs.Where(u => u.Email == userForm.Email).FirstOrDefaultAsync();
                String code = user.VerificationCode;
              
                var response = await SendEmail(user, code, "PassRecoveryTemplate.html", "Send Code");
                if (response)
                {
                    respuesta.Ok = 1;           
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

        public class RecoveryForm
        {
            public string Email { get; set; }
        }



    }
 

}
