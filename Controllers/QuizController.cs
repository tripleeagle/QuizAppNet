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
            if (_context.Quizzes.Count() == 0)
            {
                ICollection<Question> questions = new HashSet<Question>();
                Quiz quiz = new Quiz { Name = "Subtraction", Type = "Math", minPercentage = 50 };
                questions.Add( new Question{Complexity=2, questionText="4/2 = ?"} );
                _context.Quizzes.Add(quiz);

                _context.SaveChanges();
            }
        }

        [HttpGet]
        public ActionResult<List<Quiz>> GetAll()
        {
            return _context.Quizzes.ToList();
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public ActionResult<Quiz> GetById(long id)
        {
            var item = _context.Quizzes.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }
    }
}
