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
    public class QuestionChoiceController : ControllerBase{
        private readonly QuizAppContext _context;

        public QuestionChoiceController (QuizAppContext context){   
            _context = context;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<QuestionChoice>>> All()
        {
            return await _context.QuestionChoices.AsNoTracking().ToListAsync();
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<QuestionChoice>> Get(long id)
        {
            var item = await _context.QuestionChoices.FindAsync(id);
            if (item == null)
                return NotFound();
        
            return item;
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create (QuestionChoice questionChoice){
            if (!ModelState.IsValid) {
                return Forbid(StringsConf.InvalidModel);
            }
            await _context.QuestionChoices.AddAsync(questionChoice);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("update")]
        public async Task<ActionResult> Update (QuestionChoice newQuestionChoice){
            if (!ModelState.IsValid) {
                return Forbid(StringsConf.InvalidModel);
            }
            QuestionChoice oldQuestionChoice = (await Get(newQuestionChoice.Id)).Value;
            if ( oldQuestionChoice == null ) {
                await Create(newQuestionChoice);
                return Ok();
            }
            oldQuestionChoice.ChoiceText = newQuestionChoice.ChoiceText;
            oldQuestionChoice.IsRight = newQuestionChoice.IsRight;
            oldQuestionChoice.Question = newQuestionChoice.Question;
            oldQuestionChoice.QuestionId = newQuestionChoice.QuestionId;
            await _context.SaveChangesAsync();  
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete (long id){
            QuestionChoice questionChoice = await _context.QuestionChoices.FindAsync(id);
            if ( questionChoice == null )
                return NotFound();
            _context.QuestionChoices.Remove(questionChoice);
            await _context.SaveChangesAsync();  
            return Ok();
        }

        [HttpGet("question/{id}")]
        public async Task<ActionResult<Question>> GetQuiz (long id){
            QuestionChoice questionChoice = (await Get(id)).Value;
            if ( questionChoice == null )
                return NotFound();
            return await _context.Questions.FindAsync(questionChoice.QuestionId);
        }
    }
}