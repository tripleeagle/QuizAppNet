using Microsoft.EntityFrameworkCore;


namespace QuizappNet.Models
{
    public class QuizAppContext : DbContext{
         public QuizAppContext(DbContextOptions<QuizAppContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ForNpgsqlUseIdentityColumns();
            modelBuilder.Entity<QuizQuestion>().HasKey(t => new { t.QuizId, t.QuestionId });
        }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionChoice> QuestionChoices { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<QuizQuestion> QuizQuestions { get; set; }
    }
}