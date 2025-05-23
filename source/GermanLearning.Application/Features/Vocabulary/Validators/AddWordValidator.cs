using FluentValidation;
using GermanLearning.Application.Features.Vocabulary.Commands;
using GermanLearning.Domain.Enums;

namespace GermanLearning.Application.Features.Vocabulary.Validators;

public class AddWordValidator : AbstractValidator<AddWordCommand>
{
    public AddWordValidator()
    {
        RuleFor(x => x.GermanText).NotEmpty().MaximumLength(100);
        RuleFor(x => x.EnglishTranslation)
            .NotEmpty()
            .When(x => x.ExampleSentences != null);
        RuleFor(x => x.SpanishTranslation)
            .NotEmpty()
            .When(x => x.ExampleSentences != null);

        RuleFor(x => x.Type).IsInEnum();

        RuleFor(x => x.Gender)
            .NotNull()
            .When(x => x.Type == WordType.Noun)
            .WithMessage("Gender must be specified for nouns");

        RuleForEach(x => x.ExampleSentences)
            .NotEmpty()
            .When(x => x.ExampleSentences != null);

        RuleForEach(x => x.Synonyms)
            .NotEmpty()
            .When(x => x.Synonyms != null);
    }
}