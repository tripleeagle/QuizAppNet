using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizappNet.Models
{
    public class Quiz
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public double minPercentage { get; set; }
        public ICollection<QuizQuestion> QuizQuestion { get; set; } = new HashSet<QuizQuestion>();
        public ICollection<Result> Results { get; set; } = new HashSet<Result>();
    }
}