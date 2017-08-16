using UsdaCosmos;

namespace UsdaCosmosJson 
{
    public class TopNutrientJson : NutrientJson 
    {
        public FoodItemNutrientJson[] FoodItems { get; set; }

        public static TopNutrientJson FromTopNutrient(Nutrient nutrient)
        {
            return new TopNutrientJson
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