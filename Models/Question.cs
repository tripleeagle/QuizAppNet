using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizappNet.Models
{
    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public int Complexity { get; set; }
        public string QuestionText { get; set; }
        public virtual ICollection<QuizQuestion> QuizzesLink { get; set; }
        public virtual ICollection<QuestionChoice> QuestionChoices { get; set; }
    }
}