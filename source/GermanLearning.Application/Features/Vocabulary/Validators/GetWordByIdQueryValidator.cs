using FluentValidation;
using GermanLearning.Application.Features.Vocabulary.Queries;

namespace GermanLearning.Application.Features.Vocabulary.Validators;

public class GetWordByIdQueryValidator : AbstractValidator<GetWordByIdQuery>
{
    public GetWordByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Word ID is required");
    }
}