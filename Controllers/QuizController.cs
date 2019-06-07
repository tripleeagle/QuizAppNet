using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using QuizappNet.Models;

namespace QuizappNet.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase{
        private readonly QuizAppContext _context;

        public QuizController (QuizAppContext context){
            _context = context;
        }
        
        [HttpGet("GetQuizList")]
        public ActionResult<List<Quiz>> GetAll()
        {
            return _context.Quizzes.ToList();
        }

        [HttpGet("{id}", Name = "GetQuiz")]
        public ActionResult<Quiz> GetById(long id)
        {
            var item = _context.Quizzes.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpPost("AddQuiz")]
        public void Add (Quiz quiz){
            _context.Quizzes.Add(quiz);
            if ( quiz.Results.Count != 0 )
                _context.Results.AddRange(quiz.Results);
            
            _context.SaveChanges();
        }
    }
}
