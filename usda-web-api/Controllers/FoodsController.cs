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
    public class FoodsController : Controller
    {
        private IConfiguration configuration;

        public FoodsController(IConfiguration config)
        {
            this.configuration = config;
        }

        // GET api/foods
        [HttpGet]
        public async Task<IEnumerable<FoodItemJson>> GetAsync([FromQuery] string groupId = null, [FromQuery] string search = null)
        {
            if (groupId != null) 
            {
                groupId = groupId.Trim();
            }
            if (search != null)
            {
                search = search.Trim();
            }
            if (string.IsNullOrWhiteSpace(groupId) && string.IsNullOrWhiteSpace(search))
            {
                return Enumerable.Empty<FoodItemJson>();
            }
            var client = new CosmosClient();
            var db = client.ConnectAndGetDatabase(this.configuration);
            var projection = Builders<FoodItem>.Projection.Include(fi => fi.FoodId).Include(fi => fi.FoodGroupId)
                .Include(fi => fi.Description).Include(fi => fi.ShortDescription);
            var query = db.GetCollection<FoodItem>(Collections.GetCollectionName<FoodItem>());
            IFindFluent<FoodItem, FoodItem> findFluent; 
            if (string.IsNullOrWhiteSpace(groupId))
            {
                findFluent = query.Find(fi => fi.Description.Contains(search));
            }
            else if (string.IsNullOrWhiteSpace(search))
            {
                findFluent = query.Find(fi => fi.FoodGroupId == groupId);
            }
            else 
            {
                findFluent = query.Find(fi => fi.FoodGroupId == groupId && fi.Description.Contains(search));
            }
            var projectedQuery = findFluent.Limit(100).Project(projection);
            var list = new List<FoodItemJson>();
            await projectedQuery.ForEachAsync(fi => list.Add(FoodItemJson.FromFoodItem(fi)));
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
            var query = await db.GetCollection<FoodItem>(Collections.GetCollectionName<FoodItem>()).FindAsync(fi => fi.FoodId == id);
            var item = await query.FirstOrDefaultAsync();
            if (item == null)
            {
                return NotFound();
            }
            return Ok(FoodItemDetailJson.FromFoodItem(item));
        }
    }
}
