using FluentValidation;
using GermanLearning.Application.Features.Exercises.Commands;

namespace GermanLearning.Application.Features.Exercises.Validators;

public class SaveExerciseResultValidator : AbstractValidator<SaveExerciseResultCommand>
{
    public SaveExerciseResultValidator()
    {
        RuleFor(x => x.ExerciseId).NotEmpty();
        RuleFor(x => x.TotalQuestions).GreaterThan(0);
        RuleFor(x => x.CorrectAnswers)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(x => x.TotalQuestions);
    }
}