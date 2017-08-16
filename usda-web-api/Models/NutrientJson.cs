using UsdaCosmos;

namespace UsdaCosmosJson
{
    public class NutrientJson
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public decimal AmountInHundredGrams { get; set; }
        public string UnitOfMeasure { get; set; }
        public int SortOrder { get; set;}

        public static NutrientJson FromNutrient(Nutrient nutrient)
        {
            return new NutrientJson
            {
                Id = nutrient.NutrientId,
                AmountInHundredGrams = nutrient.AmountInHundredGrams,
                Description = nutrient.Definition.Description,
                UnitOfMeasure = nutrient.Definition.UnitOfMeasure,
                SortOrder = nutrient.Definition.SortOrder
            };
        }
    }
}