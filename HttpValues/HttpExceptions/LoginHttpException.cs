using Microsoft.AspNetCore.Mvc;
using QuizappNet.Values;

namespace QuizappNet.HttpValues.HttpExceptions
{
    public class LoginHttpException : HttpException
    {
        public LoginHttpException(): base(EngStrings.InvalidLoginOrPassword){}
    }
}