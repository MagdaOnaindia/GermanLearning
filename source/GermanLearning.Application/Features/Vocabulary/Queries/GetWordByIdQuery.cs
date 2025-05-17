using GermanLearning.Application.DTO.Vocabulary;
using MediatR;

namespace GermanLearning.Application.Features.Vocabulary.Queries;

public record GetWordByIdQuery(Guid Id) : IRequest<WordDto>;