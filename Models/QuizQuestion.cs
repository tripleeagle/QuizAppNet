using System.ComponentModel.DataAnnotations;

namespace QuizappNet.Models
{
    public class QuizQuestion
    {
        [Required(AllowEmptyStrings = false)]
        
        public long QuizId { get; set; }

        public Quiz Quiz { get; set; }


        [Required(AllowEmptyStrings = false)]        
        public long QuestionId { get; set; }

        public Question Question { get; set; }
    }
}