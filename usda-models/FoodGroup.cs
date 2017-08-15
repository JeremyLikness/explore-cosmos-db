using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UsdaCosmos 
{
    public class FoodGroup
    {
        [BsonId]
        public ObjectId Id { get; set;}
        public string Code { get; set; }
        public string Description { get; set; }
    }
}