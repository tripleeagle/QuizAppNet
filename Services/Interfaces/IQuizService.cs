using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuizappNet.Models;

namespace QuizappNet.Services.Interfaces
{
    public interface IQuizService
    {
         Task<ActionResult<List<Quiz>>> All();
         Task<ActionResult<Quiz>> Get(long id);
         Task<ActionResult> Create (Quiz quiz);
         Task<ActionResult> Update (Quiz newQuiz);
         Task<ActionResult> Delete (long id);
         Task<ActionResult<List<Result>>> Results(long id);
         Task<ActionResult<List<Question>>> Questions(long id);
    }
}