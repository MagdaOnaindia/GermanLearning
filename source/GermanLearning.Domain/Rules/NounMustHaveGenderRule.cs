using GermanLearning.Domain.Enums;

namespace GermanLearning.Domain.Rules;

public class NounMustHaveGenderRule : IBusinessRule
{
    private readonly WordType _type;
    private readonly Gender? _gender;

    public NounMustHaveGenderRule(WordType type, Gender? gender)
    {
        _type = type;
        _gender = gender;
    }

    public bool IsBroken() => _type == WordType.Noun && !_gender.HasValue;

    public string Message => "Nouns must have a specified gender";
}