using Microsoft.Extensions.Options;
using MongoDB.Driver;
using GermanLearning.Domain.Entities;
using GermanLearning.Infrastructure.Configurations;

namespace GermanLearning.Infrastructure.Persistence.Contexts;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoConfig> config)
    {
        var client = new MongoClient(config.Value.ConnectionString);
        _database = client.GetDatabase(config.Value.DatabaseName);
    }

    public IMongoCollection<Word> Words =>
        _database.GetCollection<Word>("words");

    public IMongoCollection<Exercise> Exercises =>
        _database.GetCollection<Exercise>("exercises");

    public IMongoCollection<ExerciseResult> ExerciseResults =>
        _database.GetCollection<ExerciseResult>("exercise_results");
}