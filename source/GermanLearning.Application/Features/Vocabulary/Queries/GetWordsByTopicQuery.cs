using GermanLearning.Application.DTO.Vocabulary;
using MediatR;

namespace GermanLearning.Application.Features.Vocabulary.Queries;

public record GetWordsByTopicQuery(string Topic) : IRequest<List<WordDto>>;