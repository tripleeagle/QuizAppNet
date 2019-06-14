using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizappNet.Models;
using QuizappNet.Utils;

namespace QuizappNet.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionController : ControllerBase{
        private readonly QuizAppContext _context;

        public QuestionController (QuizAppContext context){   
            _context = context;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Question>>> All()
        {
            return await _context.Questions.AsNoTracking().ToListAsync();
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<Question>> Get(long id)
        {
            var item = await _context.Questions.FindAsync(id);
            if (item == null)
                return NotFound();
            return item;
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create (Question question){
            if (!ModelState.IsValid) {
                return Forbid(StringsConf.InvalidModel);
            }
            await _context.Questions.AddAsync(question);
            if ( question.QuizzesLink != null ){
                foreach ( var quizLink in question.QuizzesLink ){
                    quizLink.Question = question;
                    quizLink.Quiz = await _context.Quizzes.FindAsync(quizLink.QuizId);
                }
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("update")]
        public async Task<ActionResult> Update (Question newQuestion){
            if (!ModelState.IsValid) {
                return Forbid(StringsConf.InvalidModel);
            }
            Question oldQuestion = (await Get(newQuestion.Id)).Value;
            if ( oldQuestion == null ) {
                await Create(newQuestion);
                return Ok();
            } 
            oldQuestion.Complexity = newQuestion.Complexity;
            oldQuestion.QuestionText = newQuestion.QuestionText;
            oldQuestion.QuestionChoices.Clear();
            oldQuestion.QuestionChoices = newQuestion.QuestionChoices;

            if ( oldQuestion.QuizzesLink != null )
                oldQuestion.QuizzesLink.Clear();

            if ( newQuestion.QuizzesLink != null ){
                foreach ( var quizLink in newQuestion.QuizzesLink ){
                    quizLink.Question = newQuestion;
                    quizLink.Quiz = await _context.Quizzes.FindAsync(quizLink.QuizId);
                }
            }
            oldQuestion.QuizzesLink = newQuestion.QuizzesLink;
            await _context.SaveChangesAsync();  
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete (long id){
            var question = await _context.Questions.FindAsync(id);
            if ( question == null )
                return NotFound();

            if ( question.QuizzesLink != null )
                question.QuizzesLink.Clear();
            
            if ( question.QuestionChoices != null )
                question.QuestionChoices.Clear();
            
            var questionChoices = _context.QuestionChoices.Where( r => r.QuestionId == id ).ToListAsync();
            if ( questionChoices != null ){
                _context.QuestionChoices.RemoveRange(await questionChoices);
            }

            var quizList = _context.QuizQuestions.Where( qq => qq.QuestionId == id ).ToListAsync();
            if ( quizList != null ){
                _context.QuizQuestions.RemoveRange(await quizList);
            }

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();  
            return Ok();
        }

        [HttpGet("questionChoices/{id}")]
        public async Task<ActionResult<List<QuestionChoice>>> GetQuestionChoices(long id){
            if ( await Get(id) == null )
                return NotFound();
            IQueryable<QuestionChoice> QuestionChoices = _context.QuestionChoices.Where( qc => qc.QuestionId == id ).AsNoTracking();
            return await QuestionChoices.ToListAsync();
        }
    }
}