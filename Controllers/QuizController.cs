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

        [HttpPost("CreateQuiz")]
        public void Create (Quiz quiz){
            _context.Quizzes.Add(quiz);
            if ( quiz.QuestionsLink != null ){
                foreach ( var questionLink in quiz?.QuestionsLink ){
                    questionLink.Quiz = quiz;
                    questionLink.Question = _context.Questions.Find(questionLink.QuestionId);
                }
            }
            _context.SaveChanges();  
        }
        
        [HttpPost("UpdateQuiz")]
        public void Update (Quiz newQuiz){
            var oldQuiz = _context.Quizzes.Find(newQuiz.Id);
            if ( oldQuiz == null ) {
                Create(newQuiz);
                return;
            } 
            oldQuiz.Name = newQuiz.Name;
            oldQuiz.MinPercentage = newQuiz.MinPercentage;
            oldQuiz.Results = newQuiz.Results;
            oldQuiz.Type = newQuiz.Type;

            if ( oldQuiz.QuestionsLink != null ){   
                oldQuiz.QuestionsLink.Clear();
            }

            if ( newQuiz.QuestionsLink != null ){
                foreach ( var questionLink in newQuiz.QuestionsLink ){
                    questionLink.Quiz = newQuiz;
                    questionLink.Question = _context.Questions.Find(questionLink.QuestionId);
                }
            }
            _context.SaveChanges();  
        }

        [HttpDelete("DeleteQuiz")]
        public void Delete (long id){
            var quiz = _context.Quizzes.Find(id);
            if ( quiz == null ){
                return;
            }
            if ( quiz.QuestionsLink != null ){   
                quiz.QuestionsLink.Clear();
            }
            if ( quiz.Results != null ){
                quiz.Results.Clear();
            }
            var results = _context.Results.Where( r => r.QuizId == quiz.Id ).ToList();
            if ( results != null ){
                _context.Results.RemoveRange(results);
            }

            var questionList = _context.QuizQuestions.Where( qq => qq.QuizId == quiz.Id ).ToList();
            if ( questionList != null ){
                _context.QuizQuestions.RemoveRange(questionList);
            }

            _context.Quizzes.Remove(quiz);
            _context.SaveChanges();  
        }

        [HttpGet("GetResults")]
        public ActionResult<List<Result>> GetResults(long id){
            if ( GetById(id) == null ){
                return NotFound();
            }
            var results =  _context.Results.Where( r => r.QuizId == id).ToList();
            foreach ( var result in results ){
                result.Quiz = null;
            }
            return results;
        }

        [HttpGet("GetQuestions")]
        public ActionResult<List<Question>> GetQuestions(long id)
        {
            IQueryable<QuizQuestion> quizQuestions = _context.QuizQuestions
            .Where( qq => qq.QuizId == id );
            var quizQuestionList =  quizQuestions.ToList();
            var questionList = new List<Question>();
            foreach ( var quizQuestion in quizQuestionList )
            {
                var question = _context.Questions.Find(quizQuestion.QuestionId);
                if ( question != null ){
                    question.QuizzesLink = null; //To prevent infinite recursion
                    questionList.Add( question );
                }
            }
            return questionList;
        }
    }
}
