using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizappNet.Models;

namespace QuizappNet.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultConroller : ControllerBase{
        private readonly QuizAppContext _context;

        public ResultConroller (QuizAppContext context){   
            _context = context;
        }

        [HttpGet("GetResults")]
        public async Task<ActionResult<List<Result>>> GetAll()
        {
            return await _context.Results.ToListAsync();
        }

        [HttpGet("{id}", Name = "GetResult")]
        public async Task<ActionResult<Result>> GetById(long id)
        {
            Result item = await _context.Results.FindAsync(id);
            if (item == null) 
                return NotFound();
            return item;
        }

        [HttpPost("AddResult")]
        public async Task Create (Result result){
            await _context.Results.AddAsync(result);
            await _context.SaveChangesAsync();
        }

        [HttpPost("UpdateResult")]
        public async Task Update (Result newResult){
            Result oldResult = (await GetById(newResult.Id)).Value;
            if ( oldResult == null ) {
                await Create(newResult);
                return;
            }
            oldResult.UserName = newResult.UserName;
            oldResult.Score = newResult.Score;
            oldResult.QuizId = newResult.QuizId;
            await _context.SaveChangesAsync();  
        }

        [HttpDelete("DeleteResult")]
        public async Task Delete (long id){
            Result result = await _context.Results.FindAsync(id);
            if ( result == null )
                return;
            _context.Results.Remove(result);
            await _context.SaveChangesAsync();  
        }

        [HttpGet("GetQuiz")]
        public async Task<ActionResult<Quiz>> GetQuiz (long id){
            Result result = (await GetById(id)).Value;
            if ( result == null )
                return NotFound();
            return await _context.Quizzes.FindAsync(result.QuizId);
        }
    }
}