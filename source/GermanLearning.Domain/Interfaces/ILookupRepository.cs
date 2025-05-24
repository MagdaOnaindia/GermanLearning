// GermanLearning.Domain/Repositories/Interfaces/ILookupRepository.cs (or individual ones)
// These repositories will query the tables defined by your POCOs in OnModelCreating
using GermanLearning.Domain.Entities;

namespace GermanLearning.Domain.Repositories.Interfaces;

// These interfaces are for querying the tables defined by the POCOs
// We'll use the POCOs directly in the repository implementation for querying
// as we don't have DbSet<WordTypeLookup>

public interface IWordTypeLookupRepository
{
    Task<List<WordTypeLookup>> GetAllAsync(); // Returns the POCO
}

public interface IGenderLookupRepository
{
    Task<List<GenderLookup>> GetAllAsync(); // Returns the POCO
}