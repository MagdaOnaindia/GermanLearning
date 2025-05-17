namespace GermanLearning.Infrastructure.Configurations;

public class MongoConfig
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
}