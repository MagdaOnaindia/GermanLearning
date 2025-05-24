// GermanLearning.Application/Interfaces/ILookupService.cs (or individual interfaces)
using GermanLearning.Application.DTOs.Vocabulary;

namespace GermanLearning.Application.Interfaces;

public interface IWordTypeLookupService
{
    Task<List<WordTypeLookupDto>> GetAllAsync();
}

public interface IGenderLookupService
{
    Task<List<GenderLookupDto>> GetAllAsync(bool excludeNone = true);
}