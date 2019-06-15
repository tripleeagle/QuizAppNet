using Microsoft.AspNetCore.Mvc;

namespace QuizappNet.Utils.Models
{
    public class LoginHttpException : HttpException
    {
        public LoginHttpException(): base(EngStrings.InvalidLoginOrPassword){}
    }
}