// GermanLearning.Application/Interfaces/ITopicService.cs
using GermanLearning.Application.DTO.Vocabulary; // Assuming TopicDto will be here
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GermanLearning.Application.Interfaces;

public interface ITopicService
{
    Task<TopicDto> AddTopicAsync(string name, string? description = null); // Or an AddTopicCommand
    Task UpdateTopicAsync(Guid id, string name, string? description = null); // Or an UpdateTopicCommand
    Task DeleteTopicAsync(Guid id);
    Task<TopicDto?> GetTopicByIdAsync(Guid id);
    Task<TopicDto?> GetTopicByNameAsync(string name);
    Task<List<TopicDto>> GetAllTopicsAsync();
}