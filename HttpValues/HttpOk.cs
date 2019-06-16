using Microsoft.AspNetCore.Mvc;
using QuizappNet.Values;

namespace QuizappNet.HttpValues
{
    public class HttpOk
    {
        public string Text { get; }
        public HttpOk ( ){
            this.Text = EngStrings.Success;
        }
        public JsonResult ToJson () {
            return new JsonResult(this) { StatusCode = 200 };
        }
    }
}