namespace GermanLearning.Domain.Rules;

public class TranslationNotEmptyRule : IBusinessRule
{
    private readonly List<string> _translation;
    private readonly string _language;

    public TranslationNotEmptyRule(List<string> translation, string language)
    {
        _translation = translation;
        _language = language;
    }

    public bool IsBroken() => _translation.Count()>0;

    public string Message => $"{_language} translation cannot be empty";
}