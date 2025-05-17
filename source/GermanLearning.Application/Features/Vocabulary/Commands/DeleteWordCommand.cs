using MediatR;

namespace GermanLearning.Application.Features.Vocabulary.Commands;

public record DeleteWordCommand(Guid Id) : IRequest<Unit>;