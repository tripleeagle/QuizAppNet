using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizappNet.Models
{
    public class QuestionChoice{
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        public string ChoiceText { get; set; }
        public bool IsRight { get; set; }
        public Question Question { get; set; }
    }
}