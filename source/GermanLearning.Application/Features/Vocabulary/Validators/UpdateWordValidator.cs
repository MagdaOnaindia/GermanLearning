using FluentValidation;
using GermanLearning.Application.Features.Vocabulary.Commands;
using GermanLearning.Domain.Enums;

namespace GermanLearning.Application.Features.Vocabulary.Validators;

public class UpdateWordValidator : AbstractValidator<UpdateWordCommand>
{
    public UpdateWordValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Word ID is required");

        RuleFor(x => x.GermanText)
            .NotEmpty().WithMessage("German text is required")
            .MaximumLength(100).WithMessage("German text must be less than 100 characters");

        RuleFor(x => x.EnglishTranslation)
            .NotEmpty().WithMessage("English translation is required")
            .WithMessage("English translation must be less than 100 characters");
        RuleFor(x => x.SpanishTranslation)
            .NotEmpty().WithMessage("Spanish translation is required")
            .WithMessage("Spanish translation must be less than 100 characters");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Invalid word type");

        RuleFor(x => x.Gender)
            .Must((command, gender) =>
                command.Type != WordType.Noun || gender.HasValue)
            .WithMessage("Gender is required for nouns")
            .Must(gender => !gender.HasValue || Enum.IsDefined(typeof(Gender), gender.Value))
            .When(x => x.Type == WordType.Noun)
            .WithMessage("Invalid gender value");
    }
}