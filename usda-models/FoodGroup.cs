using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UsdaCosmos 
{
    public class FoodGroup
    {
        [BsonId]
        public string Code { get; set; }
        public string Description { get; set; }
    }
}