using System.Collections;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using QuizappNet.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using QuizappNet.Utils;
using QuizappNet.Utils.Models;

namespace QuizappNet.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    
    public class QuizController : ControllerBase{
        private readonly QuizAppContext _context;

        public QuizController (QuizAppContext context){
            _context = context;
        }
        
        [HttpGet("all")]
        [Authorize]
        public async Task<ActionResult<List<Quiz>>> All()
        {
            return await _context.Quizzes.AsNoTracking().ToListAsync();
        }

        [HttpGet("get/{id}")]
        [Authorize]
        public async Task<ActionResult<Quiz>> Get(long id)
        {
            var item = await (_context.Quizzes.FindAsync(id));
            if (item == null)
                return new NotFoundHttpException(id).ToJson();
            return item;
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create (Quiz quiz){
            if (!ModelState.IsValid) {
                return new InvalidObjectHttpException().ToJson();
            }
            await _context.Quizzes.AddAsync(quiz);
            var check = User.Identity.IsAuthenticated;

            if ( quiz.QuestionsLink != null ){
                foreach ( var questionLink in quiz.QuestionsLink ){
                    questionLink.Quiz = quiz;
                    questionLink.Question = await _context.Questions.FindAsync(questionLink.QuestionId);
                }
            }
            await _context.SaveChangesAsync();  
            return Ok();
        }
        
        [HttpPost("update")]
        public async Task<ActionResult> Update (Quiz newQuiz){
            if (!ModelState.IsValid) {
                return new InvalidObjectHttpException().ToJson();;
            }

            await Delete(newQuiz.Id);
            await Create(newQuiz);
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete (long id){
            var quiz = await _context.Quizzes.FindAsync(id);
            if ( quiz == null )
                return new NotFoundHttpException(id).ToJson();

            if ( quiz.QuestionsLink != null )
                quiz.QuestionsLink.Clear();
            
            if ( quiz.Results != null )
                quiz.Results.Clear();
            
            var results = _context.Results.Where( r => r.QuizId == id ).ToListAsync();
            if ( results != null ){
                _context.Results.RemoveRange(await results);
            }

            var questionList = _context.QuizQuestions.Where( qq => qq.QuizId == id ).ToListAsync();
            if ( questionList != null ){
                _context.QuizQuestions.RemoveRange(await questionList);
            }

            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();  
            return Ok();
        }

        [HttpGet("results/{id}")]
        public async Task<ActionResult<List<Result>>> Results(long id){
            if ( (await Get(id)) == null ){
                return new NotFoundHttpException(id).ToJson();
            }
            var results =  _context.Results.Where( r => r.QuizId == id).AsNoTracking().ToListAsync();
            foreach ( var result in await results ){
                result.Quiz = null;
            }
            return await results;
        }

        [HttpGet("questions/{id}")]
        public async Task<ActionResult<List<Question>>> Questions(long id)
        {
            IQueryable<QuizQuestion> quizQuestions = _context.QuizQuestions.Where( qq => qq.QuizId == id ).AsNoTracking();
            var quizQuestionList = quizQuestions.ToListAsync();
            var questionList = new List<Question>();
            foreach ( var quizQuestion in await quizQuestionList )
            {
                var question = await _context.Questions.FindAsync(quizQuestion.QuestionId);
                if ( question != null ){
                    question.QuizzesLink = null; //To prevent infinite recursion
                    questionList.Add( question );
                }
            }
            return questionList;
        }
    }
}
