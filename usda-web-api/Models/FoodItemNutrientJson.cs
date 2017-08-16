using UsdaCosmos;

namespace UsdaCosmosJson 
{
    public class FoodItemNutrientJson : FoodItemJson 
    {
        public decimal AmountInHundredGrams { get; set; }

        public static FoodItemNutrientJson FromFoodItemNutrient(FoodItem foodItem)
        {
            return new FoodItemNutrientJson
            {
                Id = foodItem.FoodId,
                ShortDescription = foodItem.ShortDescription,
                Description = foodItem.Description,
                FoodGroupId = foodItem.FoodGroupId
            };
        }
    }
}