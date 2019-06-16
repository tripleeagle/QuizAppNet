using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizappNet.HttpValues.HttpExceptions;
using QuizappNet.HttpValues;
using QuizappNet.Models;
using QuizappNet.Services.Interfaces;

namespace QuizappNet.Services
{
    public class QuizService : IQuizService
    {
        private readonly QuizAppContext _db;

        public QuizService (QuizAppContext context){
            _db = context;
        }

        public async Task<ActionResult<List<Quiz>>> All()
        {
            return await _db.Quizzes.AsNoTracking().ToListAsync();
        }

        public async Task<ActionResult<Quiz>> Get(long id)
        {
            var item = await (_db.Quizzes.FindAsync(id));
            if (item == null)
                return new NotFoundHttpException(id).ToJson();
            return item;
        }

        public async Task<ActionResult> Create (Quiz quiz){
            await _db.Quizzes.AddAsync(quiz);

            if ( quiz.QuestionsLink != null ){
                foreach ( var questionLink in quiz.QuestionsLink ){
                    questionLink.Quiz = quiz;
                    questionLink.Question = await _db.Questions.FindAsync(questionLink.QuestionId);
                }
            }
            await _db.SaveChangesAsync();  
            return new HttpOk().ToJson();
        }
        
        [HttpPost("update")]
        public async Task<ActionResult> Update (Quiz newQuiz){
            await Delete(newQuiz.Id);
            await Create(newQuiz);
            return new HttpOk().ToJson();
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete (long id){
            var quiz = await _db.Quizzes.FindAsync(id);
            if ( quiz == null )
                return new NotFoundHttpException(id).ToJson();
            
            var results = _db.Results.Where( r => r.QuizId == id ).ToListAsync();
            if ( results != null ){
                _db.Results.RemoveRange(await results);
            }

            var questionList = _db.QuizQuestions.Where( qq => qq.QuizId == id ).ToListAsync();
            if ( questionList != null ){
                _db.QuizQuestions.RemoveRange(await questionList);
            }
            
            _db.Quizzes.Remove(quiz);
            await _db.SaveChangesAsync();  
            return new HttpOk().ToJson();
        }

        [HttpGet("results/{id}")]
        public async Task<ActionResult<List<Result>>> Results(long id){
            if ( (await Get(id)) == null ){
                return new NotFoundHttpException(id).ToJson();
            }
            var results =  _db.Results.Where( r => r.QuizId == id).AsNoTracking().ToListAsync();
            foreach ( var result in await results ){
                result.Quiz = null;
            }
            return await results;
        }

        [HttpGet("questions/{id}")]
        public async Task<ActionResult<List<Question>>> Questions(long id)
        {
            IQueryable<QuizQuestion> quizQuestions = _db.QuizQuestions.Where( qq => qq.QuizId == id ).AsNoTracking();
            var quizQuestionList = quizQuestions.ToListAsync();
            var questionList = new List<Question>();
            foreach ( var quizQuestion in await quizQuestionList )
            {
                var question = await _db.Questions.FindAsync(quizQuestion.QuestionId);
                if ( question != null ){
                    question.QuizzesLink = null; //To prevent infinite recursion
                    questionList.Add( question );
                }
            }
            return questionList;
        }
    }
}