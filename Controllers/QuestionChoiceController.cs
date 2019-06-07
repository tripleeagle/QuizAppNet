using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using QuizappNet.Models;

namespace QuizappNet.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionChoiceController : ControllerBase{
        private readonly QuizAppContext _context;

        public QuestionChoiceController (QuizAppContext context){   
            _context = context;
        }

        [HttpGet("GetQuestionChoiceList")]
        public ActionResult<List<QuestionChoice>> GetAll()
        {
            return _context.QuestionChoices.ToList();
        }

        [HttpGet("{id}", Name = "GetQuestionChoice")]
        public ActionResult<QuestionChoice> GetById(long id)
        {
            var item = _context.QuestionChoices.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpPost("AddQuestionChoice")]
        public void Add (QuestionChoice questionChoice){
            _context.QuestionChoices.Add(questionChoice);
            /*if ( questionChoice.QuestionId != null ){
                var question = _context.Questions.FirstOrDefault( q => q.Id == questionChoice.QuestionId );
                question?.QuestionChoices.Add(questionChoice);
            } */
            _context.SaveChanges();
        }
    }
}