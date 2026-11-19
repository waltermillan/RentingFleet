using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb
{
    public class MongoService
    {
        public MongoService(IOptions<MongoDbSettings> options)
        {
            BsonSerializer.RegisterSerializer(
                new GuidSerializer(GuidRepresentation.Standard));

            MongoClient = new MongoClient(options.Value.ConnectionString);

            MongoDatabase = MongoClient.GetDatabase(
                options.Value.MongoDbDatabaseName);
        }

        public MongoClient MongoClient { get; }

        public IMongoDatabase MongoDatabase { get; }
    }
}
