// GermanLearning.Application/Services/TopicService.cs
using GermanLearning.Application.DTO.Vocabulary;
using GermanLearning.Application.Interfaces;
using GermanLearning.Domain.Entities;
using GermanLearning.Domain.Repositories.Interfaces;
using GermanLearning.Application.Mappings; // For ToDto()
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation; // If you add command validation

namespace GermanLearning.Application.Services;

public class TopicService : ITopicService
{
    private readonly ITopicRepository _topicRepository;
    // Optional: Add validators for Add/Update Topic commands if you create them
    // private readonly IValidator<AddTopicCommand> _addTopicValidator;

    public TopicService(ITopicRepository topicRepository /*, IValidator<AddTopicCommand> addTopicValidator */)
    {
        _topicRepository = topicRepository;
        // _addTopicValidator = addTopicValidator;
    }

    public async Task<TopicDto> AddTopicAsync(string name, string? description = null)
    {
        // Optional: Validate input (e.g., using FluentValidation with a command object)
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Topic name cannot be empty.", nameof(name));
        }

        var existingTopic = await _topicRepository.GetByNameAsync(name);
        if (existingTopic != null)
        {
            // Or return existingTopic.ToDto(), or throw a different exception like "TopicAlreadyExistsException"
            throw new InvalidOperationException($"A topic with the name '{name}' already exists.");
        }

        var topic = new Topic(name, description); // Assumes Topic constructor handles Id generation via EntityBase
        await _topicRepository.AddAsync(topic);
        await _topicRepository.SaveAsync(); // Or rely on Unit of Work

        return topic.ToDto();
    }

    public async Task UpdateTopicAsync(Guid id, string name, string? description = null)
    {
        // Optional: Validate input
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Topic name cannot be empty.", nameof(name));
        }

        var topic = await _topicRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Topic not found.");

        // Check if another topic with the new name already exists (excluding the current one)
        var existingTopicWithNewName = await _topicRepository.GetByNameAsync(name);
        if (existingTopicWithNewName != null && existingTopicWithNewName.Id != id)
        {
            throw new InvalidOperationException($"Another topic with the name '{name}' already exists.");
        }

        topic.Update(name, description); // Assuming Topic entity has an Update method
        // _topicRepository.Update(topic); // EF Core tracks changes, so Update on repo might be redundant if GetByIdAsync returns tracked entity
        await _topicRepository.SaveAsync();
    }

    public async Task DeleteTopicAsync(Guid id)
    {
        var topic = await _topicRepository.GetByIdAsync(id);
        if (topic != null)
        {
            // Consider what happens to Words associated with this topic.
            // If WordTopic has a cascading delete on TopicId, associated entries will be removed.
            // If not, you might need to handle unlinking words or preventing deletion if words are associated.
            // For now, assuming simple delete.
            await _topicRepository.DeleteAsync(id); // Assuming ITopicRepository has DeleteAsync(Guid id)
            await _topicRepository.SaveAsync();
        }
        else
        {
            throw new KeyNotFoundException("Topic not found.");
        }
    }

    public async Task<TopicDto?> GetTopicByIdAsync(Guid id)
    {
        var topic = await _topicRepository.GetByIdAsync(id);
        return topic?.ToDto();
    }

    public async Task<TopicDto?> GetTopicByNameAsync(string name)
    {
        var topic = await _topicRepository.GetByNameAsync(name);
        return topic?.ToDto();
    }

    public async Task<List<TopicDto>> GetAllTopicsAsync()
    {
        var topics = await _topicRepository.GetAllAsync();
        return topics.Select(t => t.ToDto()).ToList();
    }
}