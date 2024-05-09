using MongoDB.Driver;
using MongoDB.Bson;
using EmailService.Models;
using Microsoft.Extensions.Options;
using DnsClient;

namespace EmailService.Services
{
    public class Logging : ILoggingService
    {
        private readonly IMongoCollection<Email> _logsCollection;

        public Logging(IOptions<MongoDbSettings> mongoDBSettings)
        {
            // trækker connection string og database navn og collectionname fra program.cs aka fra terminalen ved export. Dette er en constructor injection.
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _logsCollection = database.GetCollection<Email>(mongoDBSettings.Value.CollectionName);
        }

        public async Task LogEmailSent(Email email)
        {
            await _logsCollection.InsertOneAsync(email);
        }
    }
}
