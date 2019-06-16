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
    public class ResultService : IResultService
    {
        private readonly QuizAppContext _db;

        public ResultService (QuizAppContext context){   
            _db = context;
        }

        public async Task<ActionResult<List<Result>>> All()
        {
            return await _db.Results.AsNoTracking().ToListAsync();
        }

        public async Task<ActionResult<Result>> Get(long id)
        {
            Result item = await _db.Results.FindAsync(id);
            if (item == null) 
                return new NotFoundHttpException(id).ToJson();
            return item;
        }

        public async Task<ActionResult> Create (Result result){
            await _db.Results.AddAsync(result);
            await _db.SaveChangesAsync();
            return new HttpOk().ToJson();
        }

        public async Task<ActionResult> Update (Result newResult){
            await Delete(newResult.Id);
            await Create(newResult);
            return new HttpOk().ToJson();
        }

        public async Task<ActionResult> Delete (long id){
            Result result = await _db.Results.FindAsync(id);
            if ( result == null )
                return new NotFoundHttpException(id).ToJson();
            _db.Results.Remove(result);
            await _db.SaveChangesAsync();  
            return new HttpOk().ToJson();
        }

        public async Task<ActionResult<Quiz>> Quiz (long id){
            Result result = (await Get(id)).Value;
            if ( result == null )
                return new NotFoundHttpException(id).ToJson();
            return await _db.Quizzes.FindAsync(result.QuizId);
        }
    }
}