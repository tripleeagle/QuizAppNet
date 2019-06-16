using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuizappNet.Models;

namespace QuizappNet.Services.Interfaces
{
    public interface IQuestionChoiceService
    {
        Task<ActionResult<List<QuestionChoice>>> All();
        Task<ActionResult<QuestionChoice>> Get(long id);
        Task<ActionResult> Create (QuestionChoice questionChoice);
        Task<ActionResult> Update (QuestionChoice newQuestionChoice);
        Task<ActionResult> Delete (long id);
        Task<ActionResult<Question>> Quiz (long id);
    }
}