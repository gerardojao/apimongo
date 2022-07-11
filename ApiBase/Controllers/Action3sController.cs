using AmstelAPI.Models;
using ApiBase.Data;
using ApiBase.Models;
using ApiBase.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiBase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Action3sController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly IRepository _repository;
        private readonly IConfiguration _appsettings;
        public Action3sController(IConfiguration appsettings, AppDbContext context, IRepository repository)
        {
            _context = context;
            _repository = repository;
            _appsettings = appsettings;
        }
        //GET: api/<Action1sController>
        [HttpPost("CreateCounterAction")]
        public async Task<ActionResult> CreateCounterAction(Action3 action1Counter)
        {
            Respuesta<object> respuesta = new Respuesta<object>();
            try
            {
                action1Counter.CreatedAt = new TimeZoneChecker(_context, _appsettings).DT();
                var Id = await _repository.CreateAsync(action1Counter);

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

        [HttpGet("Counter")]
        public async Task<ActionResult> CounterAction3()
        {
            Respuesta<object> respuesta = new Respuesta<object>();
            try
            {
                var count = await _context.Action3.CountAsync();
                respuesta.Data.Add(new
                {
                    contador = count
                });

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
    }
}
