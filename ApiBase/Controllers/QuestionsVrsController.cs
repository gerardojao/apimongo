﻿using AmstelAPI.Models;
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
    public class QuestionsVrsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IRepository _repository;
        private readonly IConfiguration _appsettings;
        public QuestionsVrsController(IConfiguration appsettings, AppDbContext context, IRepository repository)
        {
            _context = context;
            _repository = repository;
            _appsettings = appsettings;
        }
        // GET: api/<QuestionsVrsController>
        [HttpGet]
        public async Task<ActionResult> GetAllQuestions()
        {
            Respuesta<object> respuesta = new Respuesta<object>();
            try
            {
                var users = await _repository.SelectAll<QuestionsVr>();
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

        // GET api/<QuestionsVrsController>/5
        [HttpGet("GetQuestion/{id}")]
        public async Task<ActionResult> GetQuestion(int id)
        {
            Respuesta<object> respuesta = new Respuesta<object>();
            try
            {
                var question = await _context.QuestionsVrs.FirstOrDefaultAsync(q => q.Id == id);
                if (question != null && question.DeletedAt == null)
                {
                    respuesta.Data.Add(new
                    {
                        question.Id,
                        question.QuestionDescription,                        
                        question.UserId                        
        
                });
                    respuesta.Ok = 1;
                    respuesta.Message = "Success";
                }
                else
                {
                    respuesta.Ok = 0;
                    respuesta.Message = "Question not found";
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

        [HttpPost]
      
        public async Task<ActionResult> PostQuestion(QuestionsVr question)
        {
            Respuesta<object> respuesta = new Respuesta<object>();
            try
            {
                question.CreatedAt = new TimeZoneChecker(_context, _appsettings).DT();
                await _repository.CreateAsync<QuestionsVr>(question);
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

        // PUT api/<QuestionsController>/5
        [HttpPut("{id}")]
       
        public async Task<ActionResult> PutQuestion(int id, QuestionsVr question)
        {
            Respuesta<object> respuesta = new Respuesta<object>();
            try
            {
                var quest = await _repository.SelectById<QuestionsVr>(id);
                if (quest != null && quest.DeletedAt == null)
                {
                    
                        
                        quest.UpdatedAt = new TimeZoneChecker(_context, _appsettings).DT();
                        await _repository.UpdateAsync<QuestionsVr>(quest);
                        respuesta.Ok = 1;
                        respuesta.Message = "Success";
                 }
                
                else
                {
                    respuesta.Ok = 0;
                    respuesta.Message = "Question not found";
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

        // DELETE api/<QuestionsController>/5
        [HttpDelete("{id}")]
       
        public async Task<ActionResult> DeleteQuestion(int id)
        {
            Respuesta<object> respuesta = new Respuesta<object>();
            try
            {
                var quest = await _repository.SelectById<QuestionsVr>(id);
                if (quest != null && quest.DeletedAt == null)
                {
                    quest.DeletedAt = new TimeZoneChecker(_context, _appsettings).DT();
                    await _repository.DeleteAsync<QuestionsVr>(quest);
                    respuesta.Ok = 1;
                    respuesta.Message = "Success";
                }
                else
                {
                    respuesta.Ok = 0;
                    respuesta.Message = "Question not found";
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