using GermanLearning.Application.DTOs.Vocabulary;
using GermanLearning.Application.Interfaces;
using GermanLearning.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GermanLearning.Application.Services;

public class WordTypeLookupService : IWordTypeLookupService
{
    private readonly IWordTypeLookupRepository _repository;

    public WordTypeLookupService(IWordTypeLookupRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<WordTypeLookupDto>> GetAllAsync()
    {
        var lookups = await _repository.GetAllAsync();
        return lookups.Select(l => new WordTypeLookupDto { Id = l.Id, Name = l.Name }).ToList();
    }
}
