using FluentValidation;
using GermanLearning.Application.Features.Vocabulary.Queries;

namespace GermanLearning.Application.Features.Vocabulary.Validators;

public class GetWordsByTypeQueryValidator : AbstractValidator<GetWordsByTypeQuery>
{
    public GetWordsByTypeQueryValidator()
    {
        RuleFor(x => x.Type)
             .IsInEnum().WithMessage("Invalid word type specified");


    }
}