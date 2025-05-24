// GermanLearning.Application/Mappings/TopicMappings.cs (or similar)
using GermanLearning.Application.DTO.Vocabulary;
using GermanLearning.Domain.Entities;

namespace GermanLearning.Application.Mappings;

public static class TopicMappings
{
    public static TopicDto ToDto(this Topic topic)
    {
        if (topic == null) return null!;

        return new TopicDto
        {
            Id = topic.Id,
            Name = topic.Name,
            Description = topic.Description
            // If you add WordCount, you'd need to ensure topic.Words is loaded
            // WordCount = topic.Words?.Count ?? 0
        };
    }
}