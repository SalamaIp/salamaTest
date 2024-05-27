using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Quiz.Data
{
    public class QuizContext : DbContext
    {
        public QuizContext(DbContextOptions<QuizContext> options)
  : base(options)
        { }
        public DbSet<Quiz.Models.Objects> Objects { get; set; }

        public DbSet<Quiz.Models.Colors> Colors { get; set; }

        public DbSet<Quiz.Models.Shapes> Shapes { get; set; }


        public DbSet<Quiz.Models.FinalResult> FinalResults { get; set; }
    }
}
