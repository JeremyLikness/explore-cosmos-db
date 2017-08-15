using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration; 
using MongoDB.Driver;
using System.Security.Authentication;

namespace UsdaCosmos
{
    public class CosmosClient
    {
        public IMongoDatabase ConnectAndGetDatabase(IConfiguration config)
        {
            var settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress(config["USDA_HOST"], 10255);
            settings.UseSsl = true;
            settings.SslSettings = new SslSettings();
            settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;

            var identity = new MongoInternalIdentity(config["USDA_DATABASE"], config["USDA_USER"]);
            var evidence = new PasswordEvidence(config["USDA_PASSWORD"]);

            settings.Credentials = new List<MongoCredential>()
            {
                new MongoCredential("SCRAM-SHA-1", identity, evidence)
            };

            var client = new MongoClient(settings);
            var db = client.GetDatabase(config["USDA_DATABASE"]);
            return db;
        }
    }
}
