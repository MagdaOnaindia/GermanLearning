using GermanLearning.Application.DTO.Vocabulary;
using GermanLearning.Domain.Enums;
using MediatR;

namespace GermanLearning.Application.Features.Vocabulary.Queries;

public record GetWordsByTypeQuery(WordType Type) : IRequest<List<WordDto>>;