using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuizappNet.Models;

namespace QuizappNet.Services.Interfaces
{
    public interface IQuestionService
    {
         Task<ActionResult<List<Question>>> All();
        Task<ActionResult<Question>> Get(long id);
        Task<ActionResult> Create (Question question);
        Task<ActionResult> Update (Question newQuestion);
        Task<ActionResult> Delete (long id);
        Task<ActionResult<List<QuestionChoice>>> QuestionChoices(long id);
    }
}