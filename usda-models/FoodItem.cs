using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace UsdaCosmos
{
    public class FoodItem
    {
        [BsonId]
        public string FoodId { get; set; }
        public string FoodGroupId { get; set; }

        public FoodGroup Group { get; set; }

        public string Description { get; set; }

        public string ShortDescription { get; set; }

        public string CommonNames { get; set; }

        public string Inedible { get; set; }

        public Weight[] Weights { get; set; }

        public BsonDocument NutrientDoc { get; set; }

        [BsonIgnore]
        public Nutrient[] Nutrients { get; set; }

        public void DeserializeNutrients(string tag = null)
        {
            var elements = this.NutrientDoc["nutrients"].AsBsonDocument;
            var list = new List<Nutrient>();
            foreach (var element in elements.Elements)
            {
                if (!string.IsNullOrWhiteSpace(tag)) {
                    if (element.Name != tag) {
                        continue;
                    }
                }
                var nutrient = new Nutrient 
                {
                    FoodId = this.FoodId,
                    NutrientId = element.Value.AsBsonDocument["id"].AsString,
                    AmountInHundredGrams = element.Value.AsBsonDocument["amount"].AsDouble,
                    Definition = new NutrientDefinition
                    {
                        NutrientId = element.Value.AsBsonDocument["id"].AsString,
                        UnitOfMeasure = element.Value.AsBsonDocument["uom"].AsString,
                        Description = element.Value.AsBsonDocument["description"].AsString,
                        TagName = element.Name,
                        SortOrder = element.Value.AsBsonDocument["sortOrder"].AsInt32
                    }
                };
                list.Add(nutrient);
            }
            this.Nutrients = list.ToArray();
        }

        public void SerializeNutrients()
        {
            var root = new BsonDocument() { {"nutrients", new BsonDocument()} };
            foreach (var nutrient in this.Nutrients)
            {
                if (string.IsNullOrWhiteSpace(nutrient.Definition.TagName))
                {
                    continue;
                }
                root["nutrients"][nutrient.Definition.TagName] =
                    new BsonDocument() {
                        {"id", nutrient.NutrientId},
                        {"amount", nutrient.AmountInHundredGrams},
                        {"description", nutrient.Definition.Description},
                        {"uom", nutrient.Definition.UnitOfMeasure},
                        {"sortOrder", nutrient.Definition.SortOrder}
                    };
            }
            this.NutrientDoc = root;
        }
    }
}