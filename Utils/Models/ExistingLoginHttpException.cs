namespace QuizappNet.Utils.Models
{
    public class ExistingLoginHttpException : HttpException
    {
        public ExistingLoginHttpException(string username) : base(EngStrings.UserAlreadyExists(username)){}
    }
}