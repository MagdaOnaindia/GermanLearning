using FluentValidation;
using GermanLearning.Application.Features.Vocabulary.Queries;

namespace GermanLearning.Application.Features.Vocabulary.Validators;

public class GetWordsByTopicQueryValidator : AbstractValidator<GetWordsByTopicQuery>
{
    public GetWordsByTopicQueryValidator()
    {
        RuleFor(x => x.Topic)
            .NotEmpty().WithMessage("Topic is required")
            .MaximumLength(50).WithMessage("Topic must be less than 50 characters");
    }
}