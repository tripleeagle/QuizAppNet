using QuizappNet.Models;

namespace QuizappNet.Services
{
    public interface IUserService
    {
        User GetByName(string name);
        void Add(User user);
        void Update(User user);
    }
}