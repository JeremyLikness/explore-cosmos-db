using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Linq;
using UsdaCosmos;
using UsdaCosmosJson;

namespace usda_web_api.Controllers
{
    [Route("api/[controller]")]
    public class NutrientsController : Controller
    {
        private IConfiguration configuration;

        public NutrientsController(IConfiguration config)
        {
            this.configuration = config;
        }

        // GET api/foods
        [HttpGet]
        public async Task<IEnumerable<NutrientDetailJson>> GetAsync()
        {
            var client = new CosmosClient();
            var db = client.ConnectAndGetDatabase(this.configuration);
            var query = db.GetCollection<NutrientDefinition>(Collections.GetCollectionName<NutrientDefinition>()).Find(_ => true);
            var list = new List<NutrientDetailJson>();
            await query.ForEachAsync(n => list.Add(NutrientDetailJson.FromNutrientDefinition(n)));
            return list.OrderBy(n => n.SortOrder);
        }

        // GET api/nutrients/top/ENERGY_KCAL
        // I am currently broken and being worked on
        [HttpGet("top/{tag}")]
        public async Task<IActionResult> GetTopFoodsAsync(string tag, [FromQuery]string groupId = null)
        {
            if (groupId != null)
            {
                groupId = groupId.Trim();
            }
            tag = tag.Trim();
            if (string.IsNullOrEmpty(tag))
            {
                return NotFound();
            }
            var client = new CosmosClient();
            var db = client.ConnectAndGetDatabase(this.configuration);
            var coll = db.GetCollection<BsonDocument>(Collections.GetCollectionName<FoodItem>());

            var result = new TopNutrientJson();
            var foodItems = new List<FoodItemNutrientJson>();

            var sort = Builders<BsonDocument>.Sort.Descending($"NutrientDoc.nutrients.{tag}.amount");
            var projection = Builders<BsonDocument>.Projection
                .Include("_id")
                .Include("FoodGroupId")
                .Include("ShortDescription")
                .Include("Description")
                .Include($"NutrientDoc.nutrients.{tag}");

            IFindFluent<BsonDocument, BsonDocument> query;

            if (string.IsNullOrEmpty(groupId))
            {
                query = coll.Find(_ => true)
                    .Project(projection)
                    .Sort(sort)
                    .Limit(100);
            }
            else
            {
                query = coll.Find(new BsonDocument {
                    {"FoodGroupId", groupId}
                }).Project(projection)
                    .Sort(sort)
                    .Limit(100);
            }

            var first = true;
            await query.ForEachAsync(fi =>
            {
                var nutrient = fi["NutrientDoc"].AsBsonDocument["nutrients"].AsBsonDocument[tag].AsBsonDocument;
                if (first)
                {

                    result.Id = nutrient["id"].AsString;
                    result.Description = nutrient["description"].AsString;
                    result.UnitOfMeasure = nutrient["uom"].AsString;
                    first = false;
                }
                var foodItemNutrient = new FoodItemNutrientJson
                {
                    Id = fi["_id"].AsString,
                    FoodGroupId = fi["FoodGroupId"].AsString,
                    Description = fi["Description"].AsString,
                    ShortDescription = fi["ShortDescription"].AsString,
                    AmountInHundredGrams = nutrient["amount"].AsDouble
                };
                foodItems.Add(foodItemNutrient);
            });

            result.FoodItems = foodItems.ToArray();
            return Ok(result);
        }
    }
}
