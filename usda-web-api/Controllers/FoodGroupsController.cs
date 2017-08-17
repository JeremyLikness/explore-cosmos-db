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
    public class FoodGroupsController : Controller
    {
        private IConfiguration configuration;

        public FoodGroupsController(IConfiguration config)
        {
            this.configuration = config;
        }

        // GET api/foodGroups
        [HttpGet]
        public async Task<IEnumerable<FoodGroupJson>> GetAsync()
        {
            var client = new CosmosClient();
            var db = client.ConnectAndGetDatabase(this.configuration);
            var query = db.GetCollection<FoodGroup>(Collections.GetCollectionName<FoodGroup>()).AsQueryable();
            var list = (await query.ToListAsync()).OrderBy(fg => fg.Code);

            return list.Select(fg => FoodGroupJson.FromFoodGroup(fg));
        }

        // GET api/foodGroups/5
        [HttpGet("{code}")]
        public async Task<IActionResult> Get(string code)
        {
            code = code.Trim();
            if (string.IsNullOrWhiteSpace(code))
            {
                return NotFound();
            }
            var client = new CosmosClient();
            var db = client.ConnectAndGetDatabase(this.configuration);
            var query = await db.GetCollection<FoodGroup>(Collections.GetCollectionName<FoodGroup>()).FindAsync(fg => fg.Code == code);
            var group = await query.FirstOrDefaultAsync();
            if (group == null)
            {
                return NotFound();
            }
            var foodsQuery = await db.GetCollection<FoodItem>(Collections.GetCollectionName<FoodItem>()).FindAsync(food => food.FoodGroupId == code);
            var list = await foodsQuery.ToListAsync();
            return Ok(FoodGroupDetailJson.FromFoodGroupAndFoodItemList(group, list));
        }
    }
}
