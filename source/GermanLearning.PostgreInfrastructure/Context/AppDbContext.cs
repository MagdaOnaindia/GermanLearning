// GermanLearning.PostgreInfrastructure/Contexts/AppDbContext.cs
using GermanLearning.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GermanLearning.PostgreInfrastructure.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Word> Words { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<ExerciseResult> ExerciseResults { get; set; } // Asegúrate de que este DbSet esté aquí

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración para la entidad Word
            modelBuilder.Entity<Word>(entity =>
            {
                // Clave Primaria (Id heredado de EntityBase)
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("uuid"); // Mapear Guid a uuid de PostgreSQL

                // Propiedades requeridas y de tipo específico
                entity.Property(e => e.GermanText).IsRequired();
                // WordType y Gender (enum) se mapean a int por defecto, lo cual está bien.

                // Mapear listas de strings a JSONB
                entity.Property(e => e.EnglishTranslation).HasColumnType("jsonb").IsRequired();
                entity.Property(e => e.SpanishTranslation).HasColumnType("jsonb").IsRequired();
                entity.Property(e => e.ExampleSentences).HasColumnType("jsonb");
                entity.Property(e => e.Synonyms).HasColumnType("jsonb");

                // Timestamps
                entity.Property(e => e.CreatedAt).IsRequired();
                // UpdatedAt es nullable, por lo que EF Core lo manejará correctamente.

                // Si Word tuviera una colección de Exercises (para navegación bidireccional en muchos-a-muchos)
                // se configuraría aquí, pero dado tu modelo actual, la relación se define desde Exercise.
            });

            // Configuración para la entidad Exercise
            modelBuilder.Entity<Exercise>(entity =>
            {
                // Clave Primaria (Id heredado de EntityBase)
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("uuid"); // Mapear Guid a uuid

                // ExerciseType y DifficultyLevel (enum) se mapean a int por defecto.

                // Relación Muchos-a-Muchos entre Exercise y Word
                // EF Core creará una tabla de unión automáticamente.
                // Si Word no tiene una propiedad de navegación List<Exercise>,
                // .WithMany() sin parámetros es suficiente para una relación unidireccional desde Exercise.
                // Usamos UsingEntity para nombrar explícitamente la tabla de unión.
                entity.HasMany(e => e.Words)
                      .WithMany() // Asumiendo que Word no tiene una propiedad de navegación List<Exercise>
                      .UsingEntity("ExerciseWord"); // Nombre explícito para la tabla de unión (ej. ExerciseId, WordId)

                entity.Property(e => e.GeneratedAt).IsRequired();
            });

            // Configuración para la entidad ExerciseResult
            modelBuilder.Entity<ExerciseResult>(entity =>
            {
                // Clave Primaria (Id definido en ExerciseResult)
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("uuid"); // Mapear Guid a uuid

                // Relación Uno-a-Muchos con Exercise
                // Un ExerciseResult pertenece a un Exercise.
                // Un Exercise puede tener muchos ExerciseResults (aunque Exercise no tenga List<ExerciseResult>).
                entity.HasOne(er => er.Exercise)         // Propiedad de navegación en ExerciseResult
                      .WithMany()                      // Lado de Exercise (sin propiedad de navegación explícita en Exercise)
                      .HasForeignKey(er => er.ExerciseId) // Clave foránea en ExerciseResult
                      .IsRequired();                   // Un ExerciseResult siempre debe tener un Exercise asociado

                entity.Property(e => e.CompletedAt).IsRequired();
            });

            // Puedes añadir aquí configuraciones globales para tipos de propiedad si lo prefieres,
            // aunque ser explícito por entidad como arriba suele ser más claro.
            // Ejemplo (no necesario si ya lo hiciste por entidad):
            // foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            // {
            //     foreach (var property in entityType.GetProperties())
            //     {
            //         if (property.ClrType == typeof(Guid) && property.Name.EndsWith("Id"))
            //         {
            //             property.SetColumnType("uuid");
            //         }
            //         // Podrías intentar generalizar jsonb pero es más complejo
            //         // ya que depende del nombre de la propiedad o atributos.
            //     }
            // }
        }
    }
}