using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizappNet.Models
{
    public class Question
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        public int Complexity { get; set; }
        public string questionText { get; set; }
        public virtual ICollection<QuizQuestion> QuizQuestion { get; set; } = new HashSet<QuizQuestion>();
        public ICollection<QuestionChoice> QuestionChoices { get; set; }
    }
}