using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizappNet.Models;
using QuizappNet.HttpValues.HttpExceptions;
using QuizappNet.Services.Interfaces;
using QuizappNet.Values;

namespace QuizappNet.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase{
        private readonly IQuestionService _questionService;

        public QuestionController (IQuestionService questionService){   
            _questionService = questionService;
        }

        [HttpGet("all")]
        [Authorize]
        public async Task<ActionResult<List<Question>>> All()
        {
            return await _questionService.All();
        }

        [HttpGet("get/{id}")]
        [Authorize]
        public async Task<ActionResult<Question>> Get(long id)
        {
            return await _questionService.Get(id);
        }

        [HttpPost("create")]
        [Authorize(Roles = GroupNames.SuperUsers + "," + GroupNames.Admins)]
        public async Task<ActionResult> Create (Question question){
            if (!ModelState.IsValid) {
                return new InvalidObjectHttpException().ToJson();
            }
            return await _questionService.Create(question);
        }

        [HttpPost("update")]
        [Authorize(Roles = GroupNames.SuperUsers + "," + GroupNames.Admins)]
        public async Task<ActionResult> Update (Question newQuestion){
            if (!ModelState.IsValid) {
                return new InvalidObjectHttpException().ToJson();
            }
            return await _questionService.Update(newQuestion);
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = GroupNames.SuperUsers + "," + GroupNames.Admins)]
        public async Task<ActionResult> Delete (long id){
            return await  _questionService.Delete(id);
        }

        [HttpGet("questionChoices/{id}")]
        [Authorize]
        public async Task<ActionResult<List<QuestionChoice>>> QuestionChoices(long id){
            return await _questionService.QuestionChoices(id);
        }
    }
}