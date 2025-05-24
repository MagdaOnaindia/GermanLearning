using GermanLearning.Domain.Entities;
using GermanLearning.Domain.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GermanLearning.PostgreInfrastructure.Repositories;

public class WordTypeLookupRepository : IWordTypeLookupRepository
{
    private readonly GermanLearning.PostgreInfrastructure.Contexts.AppDbContext _context;

    public WordTypeLookupRepository(GermanLearning.PostgreInfrastructure.Contexts.AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<WordTypeLookup>> GetAllAsync()
    {
        // EF Core knows about WordTypeLookup because it was configured in OnModelCreating
        // and it knows it maps to the "WordTypes" table.
        return await _context.Set<WordTypeLookup>().OrderBy(wt => wt.Id).ToListAsync();
    }
}