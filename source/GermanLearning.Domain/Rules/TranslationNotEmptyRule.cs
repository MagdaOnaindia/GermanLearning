namespace GermanLearning.Domain.Rules;

public class TranslationNotEmptyRule : IBusinessRule
{
    private readonly string _translation;
    private readonly string _language;

    public TranslationNotEmptyRule(string translation, string language)
    {
        _translation = translation;
        _language = language;
    }

    public bool IsBroken() => string.IsNullOrWhiteSpace(_translation);

    public string Message => $"{_language} translation cannot be empty";
}