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
        public async Task<IActionResult> GetTopFoodsAsync(string tag)
        {
            tag = tag.Trim();
            if (string.IsNullOrEmpty(tag))
            {
                return NotFound();
            }
            var client = new CosmosClient();
            var db = client.ConnectAndGetDatabase(this.configuration);
            var coll = db.GetCollection<FoodItem>(Collections.GetCollectionName<FoodItem>());

            var result = new TopNutrientJson();
            var foodItems = new List<FoodItemNutrientJson>();
            
            var query = coll.Find(_=>true).Project(fi => new FoodItem {
                FoodId=fi.FoodId,
                FoodGroupId=fi.FoodGroupId,
                ShortDescription=fi.ShortDescription,
                Description=fi.Description,
                Nutrients=fi.Nutrients.Where(n => n.Definition.TagName == tag).ToArray()
            });

            var first = true;
            await query.ForEachAsync(fi => {
                if (first) {
                    result.Id = fi.Nutrients[0].NutrientId;
                    result.Description = fi.Nutrients[0].Definition.Description;
                    result.UnitOfMeasure = fi.Nutrients[0].Definition.UnitOfMeasure;
                    first = false;
                }
                var foodItemNutrient = new FoodItemNutrientJson 
                {
                    Id = fi.FoodId,
                    FoodGroupId = fi.FoodGroupId,
                    Description = fi.Description,
                    ShortDescription = fi.ShortDescription,
                    AmountInHundredGrams = fi.Nutrients[0].AmountInHundredGrams
                };
                foodItems.Add(foodItemNutrient);
            });

            result.FoodItems = foodItems.OrderByDescending(fi => fi.AmountInHundredGrams).Take(20).ToArray();
            return Ok(result);
        }
    }
}
