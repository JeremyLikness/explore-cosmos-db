using System;

namespace UsdaCosmos
{
    public static class Collections
    {
        public static string FoodItems = "foodItems";
        public static string NutrientDefinitions = "nutrientDefinitions";

        public static string FoodGroups = "foodGroups";

        public static string GetCollectionName<T>()
        {
            if (typeof(T) == typeof(FoodItem))
            {
                return FoodItems;
            }

            if (typeof(T) == typeof(NutrientDefinition))
            {
                return NutrientDefinitions;
            }

            if (typeof(T) == typeof(FoodGroup))
            {
                return FoodGroups;
            }

            throw new Exception($"Bad type: ${typeof(T)}");
        }
    }
}