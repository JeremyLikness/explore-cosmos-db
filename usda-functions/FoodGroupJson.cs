using UsdaCosmos;

namespace usdafunctions
{
    public class FoodGroupJson
    {
        public string Code { get; set; }
        public string Description { get; set; }

        public static FoodGroupJson FromFoodGroup(FoodGroup foodGroup)
        {
            return new FoodGroupJson
            {
                Code = foodGroup.Code,
                Description = foodGroup.Description
            };
        }
    }
}
