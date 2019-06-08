using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizappNet.Models;

namespace QuizappNet.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionChoiceController : ControllerBase{
        private readonly QuizAppContext _context;

        public QuestionChoiceController (QuizAppContext context){   
            _context = context;
        }

        [HttpGet("GetQuestionChoices")]
        public async Task<ActionResult<List<QuestionChoice>>> GetAll()
        {
            return await _context.QuestionChoices.ToListAsync();
        }

        [HttpGet("GetQuestionChoice")]
        public async Task<ActionResult<QuestionChoice>> GetById(long id)
        {
            var item = await _context.QuestionChoices.FindAsync(id);
            if (item == null)
                return NotFound();
        
            return item;
        }

        [HttpPost("AddQuestionChoice")]
        public async Task Create (QuestionChoice questionChoice){
            await _context.QuestionChoices.AddAsync(questionChoice);
            await _context.SaveChangesAsync();
        }

        [HttpPost("UpdateQuestionChoice")]
        public async Task Update (QuestionChoice newQuestionChoice){
            QuestionChoice oldQuestionChoice = (await GetById(newQuestionChoice.Id)).Value;
            if ( oldQuestionChoice == null ) {
                await Create(newQuestionChoice);
                return;
            }
            oldQuestionChoice.ChoiceText = newQuestionChoice.ChoiceText;
            oldQuestionChoice.IsRight = newQuestionChoice.IsRight;
            oldQuestionChoice.Question = newQuestionChoice.Question;
            oldQuestionChoice.QuestionId = newQuestionChoice.QuestionId;
            await _context.SaveChangesAsync();  
        }

        [HttpDelete("DeleteQuestionChoice")]
        public async Task Delete (long id){
            QuestionChoice questionChoice = await _context.QuestionChoices.FindAsync(id);
            if ( questionChoice == null )
                return;
            _context.QuestionChoices.Remove(questionChoice);
            await _context.SaveChangesAsync();  
        }

        [HttpGet("GetQuestion")]
        public async Task<ActionResult<Question>> GetQuiz (long id){
            QuestionChoice questionChoice = (await GetById(id)).Value;
            if ( questionChoice == null )
                return NotFound();
            return await _context.Questions.FindAsync(questionChoice.QuestionId);
        }
    }
}