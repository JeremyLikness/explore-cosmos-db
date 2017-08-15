using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UsdaCosmos
{
    public class NutrientDefinition
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string NutrientId { get; set; }
        public string UnitOfMeasure { get; set; }
        public string TagName { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }
    }
}