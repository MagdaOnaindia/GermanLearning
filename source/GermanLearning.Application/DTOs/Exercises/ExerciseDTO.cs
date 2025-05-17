using GermanLearning.Application.DTO.Vocabulary;
using GermanLearning.Domain.Enums;

namespace GermanLearning.Application.DTOs.Exercises;

public record ExerciseDto(
    Guid Id,
    ExerciseType Type,
    DifficultyLevel Difficulty,
    List<WordDto> Words,
    DateTime GeneratedAt);