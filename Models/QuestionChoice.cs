using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizappNet.Models
{
    public class QuestionChoice{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string ChoiceText { get; set; }
        public bool IsRight { get; set; }
        public long? QuestionId { get; set; }
        public Question Question { get; set; }
    }
}