using FluentValidation;
using GermanLearning.Application.Features.Vocabulary.Commands;

namespace GermanLearning.Application.Features.Vocabulary.Validators;

public class DeleteWordValidator : AbstractValidator<DeleteWordCommand>
{
    public DeleteWordValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Word ID is required");
    }
}