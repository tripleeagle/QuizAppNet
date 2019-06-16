using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuizappNet.Models;

namespace QuizappNet.Services.Interfaces
{
    public interface IResultService
    {
         Task<ActionResult<List<Result>>> All();
         Task<ActionResult<Result>> Get(long id);
         Task<ActionResult> Create (Result result);
         Task<ActionResult> Update (Result newResult);
         Task<ActionResult> Delete (long id);
         Task<ActionResult<Quiz>> Quiz (long id);
    }
}