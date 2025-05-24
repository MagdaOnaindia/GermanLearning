using GermanLearning.Application.DTOs.Vocabulary;
using GermanLearning.Application.Interfaces;
using GermanLearning.Domain.Enums;
using GermanLearning.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GermanLearning.Application.Services;

public class GenderLookupService : IGenderLookupService
{
    private readonly IGenderLookupRepository _repository;

    public GenderLookupService(IGenderLookupRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<GenderLookupDto>> GetAllAsync(bool excludeNone = true)
    {
        var lookups = await _repository.GetAllAsync();
        var query = lookups.Select(l => new GenderLookupDto { Id = l.Id, Name = l.Name });

        if (excludeNone)
        {
            query = query.Where(dto => dto.Id != (int)Gender.None);
        }
        return query.ToList();
    }
}