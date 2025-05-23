
using GermanLearning.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace GermanLearning.SQLInfrastructure.Persistence.Contexts;

public class AppDbContext : DbContext
{
    public DbSet<Word> Words { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<ExerciseResult> ExerciseResults { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aquí podés mapear reglas específicas si lo necesitás, por ejemplo relaciones, restricciones, etc.
        base.OnModelCreating(modelBuilder);
    }
}

