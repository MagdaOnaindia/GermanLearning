using GermanLearning.Domain.Entities;
using GermanLearning.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GermanLearning.PostgreInfrastructure.Repositories;

public class GenderLookupRepository : IGenderLookupRepository
{
    private readonly GermanLearning.PostgreInfrastructure.Contexts.AppDbContext _context;

    public GenderLookupRepository(GermanLearning.PostgreInfrastructure.Contexts.AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<GenderLookup>> GetAllAsync()
    {
        return await _context.Set<GenderLookup>().OrderBy(g => g.Id).ToListAsync();
    }
}
