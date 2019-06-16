using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuizappNet.Models;

namespace QuizappNet.Services.Interfaces
{
    public interface ILoginService
    {
        ActionResult<User> Get (string username);
        Task<ActionResult> Create(User newUser);
        Task<ActionResult<List<Group>>> Groups (User user); 
        bool CheckGroups (List<Group> groups);
    }
}