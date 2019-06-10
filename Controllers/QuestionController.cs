using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizappNet.Models;

namespace QuizappNet.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionController : ControllerBase{
        private readonly QuizAppContext _context;

        public QuestionController (QuizAppContext context){   
            _context = context;
        }

        [HttpGet("GetQuestions")]
        public async Task<ActionResult<List<Question>>> GetAll()
        {
            return await _context.Questions.AsNoTracking().ToListAsync();
        }

        [HttpGet("GetQuestion")]
        public async Task<ActionResult<Question>> GetById(long id)
        {
            var item = await _context.Questions.FindAsync(id);
            if (item == null)
                return NotFound();
            return item;
        }

        [HttpPost("AddQuestion")]
        public async Task Create (Question question){
            await _context.Questions.AddAsync(question);
            if ( question.QuizzesLink != null ){
                foreach ( var quizLink in question.QuizzesLink ){
                    quizLink.Question = question;
                    quizLink.Quiz = await _context.Quizzes.FindAsync(quizLink.QuizId);
                }
            }

            await _context.SaveChangesAsync();
        }

        [HttpPost("UpdateQuestionChoice")]
        public async Task Update (Question newQuestion){
            Question oldQuestion = (await GetById(newQuestion.Id)).Value;
            if ( oldQuestion == null ) {
                await Create(newQuestion);
                return;
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
        }

        [HttpDelete("DeleteQuestion")]
        public async Task Delete (long id){
            var question = await _context.Questions.FindAsync(id);
            if ( question == null )
                return;

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
        }

        [HttpGet("GetQuestionChoices")]
        public async Task<ActionResult<List<QuestionChoice>>> GetQuestionChoices(long id){
            if ( await GetById(id) == null )
                return NotFound();
            IQueryable<QuestionChoice> QuestionChoices = _context.QuestionChoices.Where( qc => qc.QuestionId == id ).AsNoTracking();
            return await QuestionChoices.ToListAsync();
        }
    }
}