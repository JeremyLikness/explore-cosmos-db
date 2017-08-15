using UsdaCosmos;

namespace UsdaCosmosJson
{
    public class FoodItemJson
    {
        public string Id { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public static FoodItemJson FromFoodItem(FoodItem foodItem)
        {
            return new FoodItemJson
            {
                Id = foodItem.FoodId,
                ShortDescription = foodItem.ShortDescription,
                Description = foodItem.Description
            };
        }
    }
}