using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizappNet.HttpValues.HttpExceptions;
using QuizappNet.HttpValues;
using QuizappNet.Models;
using QuizappNet.Services.Interfaces;

namespace QuizappNet.Services
{
    public class QuestionChoiceService : IQuestionChoiceService
    {
        private readonly QuizAppContext _db;

        public QuestionChoiceService (QuizAppContext context){   
            _db = context;
        }

        public async Task<ActionResult<List<QuestionChoice>>> All()
        {
            return await _db.QuestionChoices.AsNoTracking().ToListAsync();
        }

        public async Task<ActionResult<QuestionChoice>> Get(long id)
        {
            var item = await _db.QuestionChoices.FindAsync(id);
            if (item == null)
                return new NotFoundHttpException(id).ToJson();
            return item;
        }

        public async Task<ActionResult> Create (QuestionChoice questionChoice){
            await _db.QuestionChoices.AddAsync(questionChoice);
            await _db.SaveChangesAsync();
            return new HttpOk().ToJson();
        }

        public async Task<ActionResult> Update (QuestionChoice newQuestionChoice){
            await Delete(newQuestionChoice.Id);
            await Create(newQuestionChoice);  
            return new HttpOk().ToJson();
        }

        public async Task<ActionResult> Delete (long id){
            QuestionChoice questionChoice = await _db.QuestionChoices.FindAsync(id);
            if ( questionChoice == null )
                return new NotFoundHttpException(id).ToJson();
            _db.QuestionChoices.Remove(questionChoice);
            await _db.SaveChangesAsync();  
            return new HttpOk().ToJson();
        }

        public async Task<ActionResult<Question>> Quiz (long id){
            QuestionChoice questionChoice = (await Get(id)).Value;
            if ( questionChoice == null )
                return new NotFoundHttpException(id).ToJson();
            return await _db.Questions.FindAsync(questionChoice.QuestionId);
        }
    }
}