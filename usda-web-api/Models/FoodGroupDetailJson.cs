using UsdaCosmos;
using System.Linq;
using System.Collections.Generic;

namespace UsdaCosmosJson
{
    public class FoodGroupDetailJson
    {
        public string Code { get; set; }
        public string Description { get; set; }

        public FoodItemJson[] Foods { get; set; }

        public static FoodGroupDetailJson FromFoodGroupAndFoodItemList(FoodGroup foodGroup, IEnumerable<FoodItem> foods)
        {
            return new FoodGroupDetailJson
            {
                Code = foodGroup.Code,
                Description = foodGroup.Description,
                Foods = foods.Select(food => FoodItemJson.FromFoodItem(food)).OrderBy(food => food.ShortDescription).ToArray()
            };
        }
    }
}