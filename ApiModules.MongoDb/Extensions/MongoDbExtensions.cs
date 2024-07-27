using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace ApiModules.MongoDb.Extensions;

public static class MongoDbExtensions
{
    public static void AddMongoDb(
        this IServiceCollection services,
        string connectionString,
        bool logQueryOnConsole = false,
        bool enumAsString = false)
    {
        services.AddSingleton(_ =>
        {
            var conventionPack = new ConventionPack
            {
                new CamelCaseElementNameConvention()
            };
            
            if (enumAsString)
                conventionPack.Add(new EnumRepresentationConvention(BsonType.String));
            
            ConventionRegistry.Register("camelCase", conventionPack, _ => true);

            var mongoUrlBuilder = new MongoUrlBuilder(connectionString);
            var settings = MongoClientSettings.FromUrl(mongoUrlBuilder.ToMongoUrl());

            if (logQueryOnConsole)
                settings.ClusterConfigurator = cb =>
                {
                    cb.Subscribe<CommandStartedEvent>(e => Debug.WriteLine($"{e.CommandName} - {e.Command}"));
                };

            return new MongoClient(settings);
        });
        services.AddScoped(sp =>
        {
            var client = sp.GetRequiredService<MongoClient>();

            return client.StartSession();
        });
        services.AddSingleton(sp =>
        {
            var client = sp.GetRequiredService<MongoClient>();

            var databaseName = MongoUrl.Create(connectionString).DatabaseName;

            return client.GetDatabase(databaseName);
        });
    }
}