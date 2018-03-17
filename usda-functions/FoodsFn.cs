
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Linq;
using Microsoft.Extensions.Configuration;
using UsdaCosmos;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace usdafunctions
{
    public static class FoodsFn
    {
        [FunctionName("FoodGroups")]
        public static async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]HttpRequest req, 
            TraceWriter log,
            ExecutionContext context)
        {
            log.Info("C# HTTP trigger function processed a request.");

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var client = new CosmosClient();
            var db = client.ConnectAndGetDatabase(config);
            var query = db.GetCollection<FoodGroup>(Collections.GetCollectionName<FoodGroup>()).AsQueryable();
            var list = (await query.ToListAsync()).OrderBy(fg => fg.Code);

            return new OkObjectResult(list.Select(fg => FoodGroupJson.FromFoodGroup(fg)));
        }
    }
}
