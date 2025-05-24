using GermanLearning.Domain.Entities;
using GermanLearning.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System; // For Enum, Cast, Select
using System.Linq; // For Enum, Cast, Select

namespace GermanLearning.PostgreInfrastructure.Contexts
{
    // POCOs can be defined here as private nested classes for encapsulation
    internal class WordTypeLookup
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    internal class GenderLookup
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class AppDbContext : DbContext
    {
        public DbSet<Word> Words { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<ExerciseResult> ExerciseResults { get; set; }
        public DbSet<Topic> Topics { get; set; }
        // No DbSet for WordTypeLookup or GenderLookup, as they are not queryable domain entities

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Define and Seed WordTypes Table ---
            modelBuilder.Entity<WordTypeLookup>(entity =>
            {
                entity.ToTable("WordTypes"); // Explicitly name the table
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);

                entity.HasData(
                    Enum.GetValues(typeof(WordType))
                        .Cast<WordType>()
                        .Select(e => new WordTypeLookup { Id = (int)e, Name = e.ToString() })
                );
            });

            // --- Define and Seed Genders Table ---
            modelBuilder.Entity<GenderLookup>(entity =>
            {
                entity.ToTable("Genders"); // Explicitly name the table
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);

                entity.HasData(
                    Enum.GetValues(typeof(Gender))
                        .Cast<Gender>()
                        .Select(e => new GenderLookup { Id = (int)e, Name = e.ToString() })
                );
            });

            // --- Configure Word Entity ---
            modelBuilder.Entity<Word>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnType("uuid"); // Correct for PostgreSQL Guid

                entity.Property(e => e.GermanText).IsRequired();

                // Map List<string> to jsonb
                entity.Property(e => e.EnglishTranslation).HasColumnType("jsonb").IsRequired();
                entity.Property(e => e.SpanishTranslation).HasColumnType("jsonb").IsRequired();
                entity.Property(e => e.ExampleSentences).HasColumnType("jsonb");
                entity.Property(e => e.Synonyms).HasColumnType("jsonb");

                // Word.Type (enum) maps to WordTypeId (int) column
                entity.Property(e => e.Type)
                      .HasConversion<int>()
                      .HasColumnName("WordTypeId") // Column in Words table
                      .IsRequired();

                // Word.Gender (nullable enum) maps to GenderId (int?) column
                entity.Property(e => e.Gender)
                      .HasConversion<int?>()
                      .HasColumnName("GenderId") // Column in Words table
                      .IsRequired(false); // Gender is nullable

                // Foreign Key Constraints to Lookup Tables
                entity.HasOne<WordTypeLookup>()
                      .WithMany()
                      .HasForeignKey("WordTypeId")
                      .HasPrincipalKey(nameof(WordTypeLookup.Id));

                entity.HasOne<GenderLookup>()
                      .WithMany()
                      .HasForeignKey("GenderId")
                      .HasPrincipalKey(nameof(GenderLookup.Id))
                      .IsRequired(false); // FK is nullable if GenderId is nullable

                // Many-to-Many relationship with Topic
                entity.HasMany(w => w.Topics)
                      .WithMany(t => t.Words)
                      .UsingEntity("WordTopic"); // Defines the join table name

                entity.Property(e => e.CreatedAt).IsRequired();
                // UpdatedAt is nullable, EF Core handles it.
            });


            // Configuración para la entidad Exercise
            modelBuilder.Entity<Exercise>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnType("uuid");

                // Many-to-Many relationship with Word
                entity.HasMany(e => e.Words)
                      .WithMany() // Assuming Word does not have a direct List<Exercise> navigation
                      .UsingEntity("ExerciseWord"); // Defines the join table name

                entity.Property(e => e.Difficulty).IsRequired(); // Assuming DifficultyLevel enum maps to int
                entity.Property(e => e.Type).IsRequired(); // Assuming ExerciseType enum maps to int
                entity.Property(e => e.GeneratedAt).IsRequired();
            });

            // Configuración para la entidad ExerciseResult
            modelBuilder.Entity<ExerciseResult>(entity =>
            {
                entity.HasKey(er => er.Id); // EntityBase provides Id, but ExerciseResult defines its own Guid Id
                entity.Property(er => er.Id).HasColumnType("uuid");

                // One-to-Many relationship with Exercise
                entity.HasOne(er => er.Exercise)
                      .WithMany() // Assuming Exercise does not have List<ExerciseResult>
                      .HasForeignKey(er => er.ExerciseId)
                      .IsRequired();

                entity.Property(er => er.CorrectAnswers).IsRequired();
                entity.Property(er => er.TotalQuestions).IsRequired();
                entity.Property(er => er.CompletedAt).IsRequired();
            });

            // Configuración para la entidad Topic
            modelBuilder.Entity<Topic>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Id).HasColumnType("uuid");
                entity.Property(t => t.Name).IsRequired().HasMaxLength(100); // Example max length
                entity.HasIndex(t => t.Name).IsUnique(); // Good practice for topic names

                // Description is nullable string, EF Core handles it.
                // The many-to-many with Word is already defined from Word's perspective.
            });
        }
    }
}