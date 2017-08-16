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
    public class FoodsController : Controller
    {
        private IConfiguration configuration;

        public FoodsController(IConfiguration config)
        {
            this.configuration = config;
        }

        // GET api/foods
        [HttpGet]
        public async Task<IEnumerable<FoodItemJson>> GetAsync([FromQuery] string groupId = null)
        {
            groupId = groupId.Trim();
            if (string.IsNullOrWhiteSpace(groupId))
            {
                return Enumerable.Empty<FoodItemJson>();
            }
            var client = new CosmosClient();
            var db = client.ConnectAndGetDatabase(this.configuration);
            var query = db.GetCollection<FoodItem>("foodItems").Find(fi => fi.FoodGroupId == groupId);
            var list = new List<FoodItemJson>();
            await query.ForEachAsync(fi => list.Add(FoodItemJson.FromFoodItem(fi)));
            return list.OrderBy(fi => fi.Description);
          }

        // GET api/food/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            id = id.Trim();
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }
            var client = new CosmosClient();
            var db = client.ConnectAndGetDatabase(this.configuration);
            var query = await db.GetCollection<FoodItem>("foodItems").FindAsync(fi => fi.FoodId == id);
            var item = await query.FirstOrDefaultAsync();
            if (item == null)
            {
                return NotFound();
            }
            return Ok(FoodItemDetailJson.FromFoodItem(item));
        }
    }
}
