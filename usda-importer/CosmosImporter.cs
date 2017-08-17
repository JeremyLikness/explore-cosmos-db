using System;
using MongoDB.Driver;
using System.Security.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace UsdaCosmos
{
    public class CosmosImporter
    {
        private async Task Import<T>(IConfigurationRoot configuration, string collectionName, T[] items) 
        {
            var client = new CosmosClient();
            var db = client.ConnectAndGetDatabase(configuration);
            
            if (db.GetCollection<T>(collectionName) != null)
            {
                Console.WriteLine($"Dropping and re-creating collection {collectionName}");
                db.DropCollection(collectionName);
                db.CreateCollection(collectionName);
            }
            var itemCount = 0;
            var collection = db.GetCollection<T>(collectionName);
            Console.WriteLine($"Importing {items.Length} items into collection {collectionName}...");
            foreach(var item in items)
            {
                await collection.InsertOneAsync(item);
                itemCount += 1;
            }
            Console.WriteLine($"Successfully imported {itemCount} documents. Your collection is ready!");
        }
        public async Task ImportGroups(IConfigurationRoot config, FoodGroup[] groups)
        {
            await this.Import<FoodGroup>(config, Collections.GetCollectionName<FoodGroup>(), groups);
        }

        public async Task ImportDefinitions(IConfigurationRoot config, NutrientDefinition[] definitions)
        {
            await this.Import<NutrientDefinition>(config, Collections.GetCollectionName<NutrientDefinition>(), definitions);
        }

        public async Task ImportFood(IConfigurationRoot config, FoodItem[] foods)
        {
            await this.Import<FoodItem>(config, Collections.GetCollectionName<FoodItem>(), foods);
        }
    }
}