using FluentValidation;
using GermanLearning.Application.Features.Exercises.Commands;

namespace GermanLearning.Application.Features.Exercises.Validators;

public class GenerateExerciseValidator : AbstractValidator<GenerateExerciseCommand>
{
    public GenerateExerciseValidator()
    {
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.Difficulty).IsInEnum();
        RuleFor(x => x.QuestionCount)
            .GreaterThan(0)
            .When(x => x.QuestionCount.HasValue);
    }
}