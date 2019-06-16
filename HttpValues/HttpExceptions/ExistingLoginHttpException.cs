using QuizappNet.Values;

namespace QuizappNet.HttpValues.HttpExceptions
{
    public class ExistingLoginHttpException : HttpException
    {
        public ExistingLoginHttpException(string username) : base(EngStrings.UserAlreadyExists(username)){}
    }
}