using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
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
            var query = db.GetCollection<FoodItem>(Collections.GetCollectionName<FoodItem>())
                .Find(fi => fi.Nutrients.Any(n => n.Definition.TagName == tag))
                .Project(Builders<FoodItem>.Projection.Include(fi=>fi.FoodId).Include(fi=>fi.ShortDescription).Include(fi=>fi.FoodGroupId)
                    .Include(fi=>fi.Description).Exclude(fi=>fi.Weights).Include(fi => fi.Nutrients.Select(n => n.Definition.TagName == tag)))
                .SortByDescending(fi => fi.Nutrients.First(n => n.Definition.TagName == tag).AmountInHundredGrams)
                .Limit(20);
            var queryList = await query.ToListAsync();
            return Ok();
        }
    }
}
