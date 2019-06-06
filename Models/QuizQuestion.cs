namespace QuizappNet.Models
{
    public class QuizQuestion
    {
        public long QuizId { get; set; }
        public Quiz Quiz { get; set; }

        public long QuestionId { get; set; }
        public Question Question { get; set; }
    }
}