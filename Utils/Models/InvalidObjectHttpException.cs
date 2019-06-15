using Microsoft.AspNetCore.Mvc;

namespace QuizappNet.Utils.Models
{
    public class InvalidObjectHttpException : HttpException
    {
        public InvalidObjectHttpException(): base(EngStrings.InvalidObject) {}
    }
}