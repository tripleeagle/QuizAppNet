using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizappNet.Models
{
    public class Quiz
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public double MinPercentage { get; set; }
        public virtual ICollection<QuizQuestion> QuestionsLink { get; set; }
        public virtual ICollection<Result> Results { get; set; }
    }
}