using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizappNet.Models;
using QuizappNet.HttpValues.HttpExceptions;
using QuizappNet.Services.Interfaces;

namespace QuizappNet.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionController : ControllerBase{
        private readonly IQuestionService _questionService;

        public QuestionController (IQuestionService questionService){   
            _questionService = questionService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Question>>> All()
        {
            return await _questionService.All();
        }

        public async Task<ActionResult<Question>> Get(long id)
        {
            return await _questionService.Get(id);
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create (Question question){
            if (!ModelState.IsValid) {
                return new InvalidObjectHttpException().ToJson();
            }
            return await _questionService.Create(question);
        }

        [HttpPost("update")]
        public async Task<ActionResult> Update (Question newQuestion){
            if (!ModelState.IsValid) {
                return new InvalidObjectHttpException().ToJson();
            }
            return await _questionService.Update(newQuestion);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete (long id){
            return await  _questionService.Delete(id);
        }

        [HttpGet("questionChoices/{id}")]
        public async Task<ActionResult<List<QuestionChoice>>> QuestionChoices(long id){
            return await _questionService.QuestionChoices(id);
        }
    }
}