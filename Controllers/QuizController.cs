using System.Collections;
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

        [HttpGet("GetQuiz")]
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
            foreach ( var questionLink in quiz.QuestionsLink ){
                questionLink.Quiz = quiz;
                questionLink.Question = _context.Questions.Find(questionLink.QuestionId);
            }
            _context.SaveChanges();
        }

        [HttpGet("GetQuestionsByQuizId")]
        public ActionResult<List<Question>> GetQuestions(long id)
        {
            IQueryable<QuizQuestion> quizQuestions = _context.QuizQuestions
            .Where( qq => qq.QuizId == id );
            var quizQuestionList =  quizQuestions.ToList();
            var questionList = new List<Question>();
            foreach ( var quizQuestion in quizQuestionList ){
                var question = _context.Questions.Find(quizQuestion.QuestionId);
                if ( question != null ){
                    question.QuizzesLink = null;
                    questionList.Add( question );
                }
            }

            return questionList;
        }
    }
}
