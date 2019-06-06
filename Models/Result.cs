using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizappNet.Models
{
    public class Result
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        public string UserName { get; set; }
        public double Score { get; set; }
        public Quiz Quiz { get; set; }
    }
}