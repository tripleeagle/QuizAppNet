using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace QuizappNet.HttpValues.HttpExceptions
{
    public abstract class HttpException
    {
        public string ExceptionText { get; }
        public HttpException ( string ExceptionText ){
            this.ExceptionText = ExceptionText;
        }

        public JsonResult ToJson () {
            return new JsonResult(this) { StatusCode = 400 };
        }
    }
}