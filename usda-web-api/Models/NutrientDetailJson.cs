using UsdaCosmos;

namespace UsdaCosmosJson
{
    public class NutrientDetailJson
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string TagName { get; set; }
        public int SortOrder { get; set;}
        public string UnitOfMeasure { get; set;}

        public static NutrientDetailJson FromNutrientDefinition(NutrientDefinition definition)
        {
            return new NutrientDetailJson
            {
                Id = definition.NutrientId,
                Description = definition.Description,
                TagName = definition.TagName,
                SortOrder = definition.SortOrder,
                UnitOfMeasure = definition.UnitOfMeasure
            };
        }
    }
}