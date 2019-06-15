using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizappNet.Models;
using QuizappNet.Utils;
using QuizappNet.Utils.Models;

namespace QuizappNet.Controllers{
    [Route("api/[controller]")]
    [ApiController]

    public class ResultController : ControllerBase{
        private readonly QuizAppContext _context;

        public ResultController (QuizAppContext context){   
            _context = context;
        }

        [HttpGet("all")]
        [Authorize]
        public async Task<ActionResult<List<Result>>> All()
        {
            return await _context.Results.AsNoTracking().ToListAsync();
        }

        [HttpGet("get/{id}")]
        [Authorize]
        public async Task<ActionResult<Result>> Get(long id)
        {
            Result item = await _context.Results.FindAsync(id);
            if (item == null) 
                return new NotFoundHttpException(id).ToJson();
            return item;
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<ActionResult> Create (Result result){
            if (!ModelState.IsValid) {
                return new InvalidObjectHttpException().ToJson();;
            }
            await _context.Results.AddAsync(result);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("update")]
        [Authorize]
        public async Task<ActionResult> Update (Result newResult){
            if (!ModelState.IsValid) {
                return new InvalidObjectHttpException().ToJson();;
            }
            await Delete(newResult.Id);
            await Create(newResult);
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<ActionResult> Delete (long id){
            Result result = await _context.Results.FindAsync(id);
            if ( result == null )
                return new NotFoundHttpException(id).ToJson();
            _context.Results.Remove(result);
            await _context.SaveChangesAsync();  
            return Ok();
        }

        [HttpGet("quiz/{id}")]
        public async Task<ActionResult<Quiz>> Quiz (long id){
            Result result = (await Get(id)).Value;
            if ( result == null )
                return new NotFoundHttpException(id).ToJson();
            return await _context.Quizzes.FindAsync(result.QuizId);
        }
    }
}