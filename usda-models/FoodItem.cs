using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UsdaCosmos
{
    public class FoodItem
    {
        [BsonId]
        public ObjectId Id { get; set;}

        public string FoodId { get; set; }
        public string FoodGroupId { get; set; }

        public FoodGroup Group { get; set; }

        public string Description { get; set; }

        public string ShortDescription { get; set; }

        public string CommonNames { get; set; }

        public string Inedible { get; set; }

        public Weight[] Weights { get; set; }

        public Nutrient[] Nutrients { get; set; }
    }
}