using System.Net.Http;
using System.Threading.Tasks;

namespace UsdaCosmos
{
    public class Agent
    {
        public async Task<string> LoadUrlAsync(string url) 
        {
            using (var client = new HttpClient())
            {
                return await client.GetStringAsync(url);
            }
        }
    }
}