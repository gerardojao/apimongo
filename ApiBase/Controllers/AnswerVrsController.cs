using ApiBase.Models;
using ApiBase.Data;
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
    public class AnswerVrsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepository _repository;
        private readonly IConfiguration _appsettings;
        public AnswerVrsController(IConfiguration appsettings, AppDbContext context, IRepository repository)
        {
            _context = context;
            _repository = repository;
            _appsettings = appsettings;
        }
        // GET: api/<ValuesController>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAnswerbyId(int id)
        {
            Respuesta<object> respuesta = new Respuesta<object>();
            try
            {
                var ans = await (from _answer in _context.AnswersVrs
                               join _question in _context.QuestionsVrs on _answer.QuestionId equals _question.Id
                               join _user in _context.UsersVrs on _answer.UserId equals _user.Id
                                where _answer.Id == id
                               select new
                               {
                                   _answer.Id,
                                   _answer.AnswerDescription,                                  
                                    user = _user.Email,
                                   question = _question.QuestionDescription
                               }).FirstOrDefaultAsync();
                if (ans != null)
                {
                    respuesta.Data.Add(ans);
                    respuesta.Ok = 1;
                    respuesta.Message = "Answer found";
                }
                else
                {
                    respuesta.Ok = 0;
                    respuesta.Message = "Answer not found";
                }
            }
            catch (Exception e)
            {
                respuesta.Ok = 0;
                respuesta.Message = e.Message + " " + e.InnerException;
            }
            return Ok(respuesta);
        }

        [HttpPost]

        public async Task<ActionResult> PostAnswer(AnswersVr answer)
        {
            Respuesta<object> respuesta = new Respuesta<object>();
            try
            {
                await _repository.CreateAsync(answer);
                respuesta.Ok = 1;
                respuesta.Message = "Success";
            }
            catch (Exception e)
            {
                respuesta.Ok = 0;
                respuesta.Message = e.Message + " " + e.InnerException;
                return Ok(respuesta);
            }
            return Ok(respuesta);
        }


    }
}
