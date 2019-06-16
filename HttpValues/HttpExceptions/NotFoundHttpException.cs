using Microsoft.AspNetCore.Mvc;
using QuizappNet.Values;

namespace QuizappNet.HttpValues.HttpExceptions
{
    public class NotFoundHttpException : HttpException
    {
        public NotFoundHttpException ( long id ): base (EngStrings.NotFound(id)){}
    }
}