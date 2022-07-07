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
                                   _answer.CreatedAt,
                                   _answer.UpdateAt,
                                   _answer.DeleteAt,
                                   user = _user.Email,
                                   question = _question.QuestionDescription
                               }).FirstOrDefaultAsync();
                if (ans != null)
                {
                    if (ans.DeleteAt == null)
                    {
                        respuesta.Data.Add(ans);
                    }

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

        [HttpPut("{id}")]

        public async Task<ActionResult> UpdateANswer(int id, AnswersVr answer)
        {
            Respuesta<object> respuesta = new Respuesta<object>();
            try
            {
                var ans = await _context.AnswersVrs.Where(a => a.Id == id).FirstOrDefaultAsync();
                if (ans != null && ans.DeleteAt == null)
                {
                    if (answer.AnswerDescription != ans.AnswerDescription)
                    { ans.AnswerDescription = answer.AnswerDescription; }
                    if (answer.UserId != ans.UserId)
                    { ans.UserId = answer.UserId; }
                    if (answer.QuestionId != ans.QuestionId)
                    { ans.QuestionId = answer.QuestionId; }

                    ans.UpdateAt = new TimeZoneChecker(_context, _appsettings).DT();
                    await _repository.UpdateAsync(ans);
                    respuesta.Ok = 1;
                    respuesta.Message = " Answer updated Successfully";
                }

                else
                {
                    respuesta.Ok = 0;
                    respuesta.Message = "Answer not found";
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAnswer(int id)
        {
            Respuesta<object> respuesta = new Respuesta<object>();
            try
            {
                var ans = await _repository.SelectById<AnswersVr>(id);
                if (ans != null && ans.DeleteAt == null)
                {
                    ans.DeleteAt = new TimeZoneChecker(_context, _appsettings).DT();
                    await _repository.DeleteAsync(ans);
                    respuesta.Ok = 1;
                    respuesta.Message = "Deleted Successfully";
                }
                else
                {
                    respuesta.Ok = 0;
                    respuesta.Message = "Answer not found";
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
    }
}
