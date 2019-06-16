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
    public class QuestionService : IQuestionService
    {
        private readonly QuizAppContext _db;

        public QuestionService (QuizAppContext context){   
            _db = context;
        }

        public async Task<ActionResult<List<Question>>> All()
        {
            return await _db.Questions.AsNoTracking().ToListAsync();
        }

        public async Task<ActionResult<Question>> Get(long id)
        {
            var item = await _db.Questions.FindAsync(id);
            if (item == null)
                return new NotFoundHttpException(id).ToJson();
            return item;
        }

        public async Task<ActionResult> Create (Question question)
        {
            await _db.Questions.AddAsync(question);
            if ( question.QuizzesLink != null ){
                foreach ( var quizLink in question.QuizzesLink ){
                    quizLink.Question = question;
                    quizLink.Quiz = await _db.Quizzes.FindAsync(quizLink.QuizId);
                }
            }

            await _db.SaveChangesAsync();
            return new HttpOk().ToJson();
        }

        public async Task<ActionResult> Update (Question newQuestion)
        {
            await Delete (newQuestion.Id);
            await Create (newQuestion);
            return new HttpOk().ToJson();
        }

        public async Task<ActionResult> Delete (long id)
        {
            var question = await _db.Questions.FindAsync(id);
            if ( question == null )
                return new NotFoundHttpException(id).ToJson();

            var questionChoices = _db.QuestionChoices.Where( r => r.QuestionId == id ).ToListAsync();
            if ( questionChoices != null ){
                _db.QuestionChoices.RemoveRange(await questionChoices);
            }

            var quizList = _db.QuizQuestions.Where( qq => qq.QuestionId == id ).ToListAsync();
            if ( quizList != null ){
                _db.QuizQuestions.RemoveRange(await quizList);
            }
            
            _db.Questions.Remove(question);
            await _db.SaveChangesAsync();  
            return new HttpOk().ToJson();
        }

        public async Task<ActionResult<List<QuestionChoice>>> QuestionChoices(long id)
        {
            if ( await Get(id) == null )
                return new NotFoundHttpException(id).ToJson();
            IQueryable<QuestionChoice> QuestionChoices = _db.QuestionChoices.Where( qc => qc.QuestionId == id ).AsNoTracking();
            return await QuestionChoices.ToListAsync();
        }
    }
}