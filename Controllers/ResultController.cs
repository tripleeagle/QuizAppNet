using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using QuizappNet.Models;

namespace QuizappNet.Controllers{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultConroller : ControllerBase{
        private readonly QuizAppContext _context;

        public ResultConroller (QuizAppContext context){   
            _context = context;
        }

        [HttpGet("GetResultList")]
        public ActionResult<List<Result>> GetAll()
        {
            return _context.Results.ToList();
        }

        [HttpGet("{id}", Name = "GetResult")]
        public ActionResult<Result> GetById(long id)
        {
            var item = _context.Results.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpPost("AddResult")]
        public void Add (Result result){
            _context.Results.Add(result);
            _context.SaveChanges();
        }
    }
}