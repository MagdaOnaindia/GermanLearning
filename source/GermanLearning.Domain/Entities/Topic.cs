// GermanLearning.Domain/Entities/Topic.cs
namespace GermanLearning.Domain.Entities;

public class Topic : EntityBase // Asumiendo que también quieres un Id y otros campos base
{
    public string Name { get; private set; }
    public string? Description { get; private set; } // Opcional

    // Lista de palabras asociadas a este tema (para navegación bidireccional)
    // Es importante inicializarla para evitar NullReferenceException
    public List<Word> Words { get; private set; } = new List<Word>();

    // Constructor privado para EF Core
    private Topic() { }

    public Topic(string name, string? description = null)
    {
        // Id se genera en EntityBase
        Name = name; // Validar que no sea nulo o vacío
        Description = description;
        // Words se inicializa vacía
    }

    public void Update(string name, string? description = null)
    {
        Name = name;
        Description = description;
        // No se actualizan las Words directamente aquí, eso se maneja a través de la relación
    }
}