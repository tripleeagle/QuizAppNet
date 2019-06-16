using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizappNet.HttpValues.HttpExceptions;
using QuizappNet.Models;
using QuizappNet.Services.Interfaces;

namespace QuizappNet.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultController : ControllerBase{
        private readonly IResultService _resultService;

        public ResultController (IResultService resultService){   
            _resultService = resultService;
        }

        [HttpGet("all")]
        [Authorize]
        public async Task<ActionResult<List<Result>>> All()
        {
            return await _resultService.All();
        }

        [HttpGet("get/{id}")]
        [Authorize]
        public async Task<ActionResult<Result>> Get(long id)
        {
            return await _resultService.Get(id);
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<ActionResult> Create (Result result){
            if (!ModelState.IsValid) {
                return new InvalidObjectHttpException().ToJson();
            }
            return await _resultService.Create(result);
        }

        [HttpPost("update")]
        [Authorize]
        public async Task<ActionResult> Update (Result newResult){
            if (!ModelState.IsValid) {
                return new InvalidObjectHttpException().ToJson();
            }
            return await _resultService.Update(newResult);
        }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<ActionResult> Delete (long id){ 
            return await _resultService.Delete(id);
        }

        [HttpGet("quiz/{id}")]
        public async Task<ActionResult<Quiz>> Quiz (long id){
            return await _resultService.Quiz(id);
        }
    }
}