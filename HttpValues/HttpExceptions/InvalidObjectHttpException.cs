using Microsoft.AspNetCore.Mvc;
using QuizappNet.Values;

namespace QuizappNet.HttpValues.HttpExceptions
{
    public class InvalidObjectHttpException : HttpException
    {
        public InvalidObjectHttpException(): base(EngStrings.InvalidObject) {}
    }
}