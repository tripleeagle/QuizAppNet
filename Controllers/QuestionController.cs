using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using QuizappNet.Models;

namespace QuizappNet.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase{
        private readonly QuizAppContext _context;

        public QuestionController (QuizAppContext context){   
            _context = context;
        }

        [HttpGet("GetQuestionList")]
        public ActionResult<List<Question>> GetAll()
        {
            return _context.Questions.ToList();
        }

        [HttpGet("GetQuestionById")]
        public ActionResult<Question> GetById(long id)
        {
            var item = _context.Questions.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpGet("GetQuestionChoiceList")]
        public ActionResult<List<QuestionChoice>> GetQuestionChoiceList(long id){
            if ( GetById(id) == null ){
                return NotFound();
            }
            IQueryable<QuestionChoice> QuestionChoices = _context.QuestionChoices
            .Where( qc => qc.QuestionId == id );
            return QuestionChoices.ToList();
        }

        [HttpPost("AddQuestion")]
        public void Add (Question question){
            _context.Questions.Add(question);
            _context.SaveChanges();
        }


    }
}