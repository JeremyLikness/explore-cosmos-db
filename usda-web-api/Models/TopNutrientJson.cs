using UsdaCosmos;

namespace UsdaCosmosJson 
{
    public class TopNutrientJson
    {
        public string Id { get; set; }
        public string Description { get; set; }

        public string UnitOfMeasure { get; set; }
        public FoodItemNutrientJson[] FoodItems { get; set; }
    }
}