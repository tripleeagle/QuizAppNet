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
    public class QuestionChoiceController : ControllerBase{
        private readonly IQuestionChoiceService _questionChoiceService;

        public QuestionChoiceController (IQuestionChoiceService questionChoiceService){   
            _questionChoiceService = questionChoiceService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<QuestionChoice>>> All()
        {
            return await _questionChoiceService.All();
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<QuestionChoice>> Get(long id)
        {
            return await _questionChoiceService.Get(id);
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create (QuestionChoice questionChoice){
            if (!ModelState.IsValid) {
                return new InvalidObjectHttpException().ToJson();
            }
            return await _questionChoiceService.Create(questionChoice);
        }

        [HttpPost("update")]
        public async Task<ActionResult> Update (QuestionChoice newQuestionChoice){
            if (!ModelState.IsValid) {
                return new InvalidObjectHttpException().ToJson();
            }
            return await _questionChoiceService.Update(newQuestionChoice);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete (long id){
            return await _questionChoiceService.Delete(id);
        }

        [HttpGet("question/{id}")]
        public async Task<ActionResult<Question>> Quiz (long id){
            return await _questionChoiceService.Quiz(id);
        }
    }
}