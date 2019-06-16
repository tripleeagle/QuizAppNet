using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using QuizappNet.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using QuizappNet.HttpValues.HttpExceptions;
using QuizappNet.Services.Interfaces;

namespace QuizappNet.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    
    public class QuizController : ControllerBase{
        private readonly IQuizService _quizService;

        public QuizController (IQuizService quizService){
            _quizService = quizService;
        }
        
        [HttpGet("all")]
        [Authorize]
        public async Task<ActionResult<List<Quiz>>> All()
        {
            return await _quizService.All();
        }

        [HttpGet("get/{id}")]
        [Authorize]
        public async Task<ActionResult<Quiz>> Get(long id)
        {
            return await _quizService.Get(id);
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create (Quiz quiz){
            if (!ModelState.IsValid) {
                return new InvalidObjectHttpException().ToJson();
            }
            return await _quizService.Create(quiz);
        }
        
        [HttpPost("update")]
        public async Task<ActionResult> Update (Quiz newQuiz){
            if (!ModelState.IsValid) {
                return new InvalidObjectHttpException().ToJson();;
            }
            return await _quizService.Update(newQuiz);
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete (long id){
            return await _quizService.Delete(id);
        }

        [HttpGet("results/{id}")]
        public async Task<ActionResult<List<Result>>> Results(long id){
            return await _quizService.Results(id);
        }

        [HttpGet("questions/{id}")]
        public async Task<ActionResult<List<Question>>> Questions(long id)
        {
            return await _quizService.Questions(id);
        }
    }
}
