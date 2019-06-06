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
        public string questionText { get; set; }
        public virtual ICollection<QuizQuestion> QuizQuestion { get; set; } = new HashSet<QuizQuestion>();
        public ICollection<QuestionChoice> QuestionChoices { get; set; }
    }
}