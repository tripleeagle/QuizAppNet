using Microsoft.AspNetCore.Mvc;
using QuizappNet.Utils.Models;

namespace QuizappNet.Utils
{
    public class NotFoundHttpException : HttpException
    {
        public NotFoundHttpException ( long id ): base (EngStrings.NotFound(id)){}
    }
}