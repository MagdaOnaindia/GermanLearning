// GermanLearning.Application/DTO/Vocabulary/TopicDto.cs
using System;

namespace GermanLearning.Application.DTO.Vocabulary;

public class TopicDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    // You might add a count of words associated with this topic, etc.
    // public int WordCount { get; set; }
}