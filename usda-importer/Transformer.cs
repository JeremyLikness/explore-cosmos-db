using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UsdaCosmos
{
    public class Transformer 
    {
        public async Task<T[]> Transform<T>(string url, Func<string[], T> transformation) where T: new()
        {
            var agent = new Agent();
            var source = await agent.LoadUrlAsync(url);
            var parser = new Parser();
            var documents = parser.Parse(source);
            var transformedDocuments = new List<T>();
            foreach(var document in documents)
            {
                transformedDocuments.Add(transformation(document));
            }
            return transformedDocuments.ToArray();
        }
    }
}