using UsdaCosmos;
using System.Linq;
using System.Collections.Generic;

namespace UsdaCosmosJson
{
    public class FoodItemDetailJson
    {
        public string Id { get; set; }

        public string ShortDescription { get; set; }
        public string Description { get; set; }

        public string CommonNames { get; set; }

        public NutrientJson[] Nutrients { get; set; }

        public FoodGroupJson FoodGroup { get; set;}

        public Weight[] Weights { get; set;}

        public static FoodItemDetailJson FromFoodItem(FoodItem item)
        {
            item.DeserializeNutrients();
            return new FoodItemDetailJson
            {
                Id = item.FoodId,
                ShortDescription = item.ShortDescription,
                Description = item.Description,
                CommonNames = item.CommonNames,
                Nutrients = item.Nutrients.Select(n => NutrientJson.FromNutrient(n)).OrderBy(n => n.SortOrder).ToArray(),
                Weights = item.Weights
            };
        }
    }
}