using System;
using System.Linq;
using FoodAndRecipeManager.Models;

namespace FoodAndRecipeManager
{
    internal static class IngredientCommands
    {
        public static void AddIngredientInteractive(RecipeDatabase db)
        {
            Console.WriteLine("\nAdding a new ingredient.");

            Console.Write("Name: ");
            string? name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Cancelled: ingredient name cannot be empty.");
                return;
            }

            // prevent duplicate by name
            bool exists = db.Ingredients.Any(i =>
                i.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));

            if (exists)
            {
                Console.WriteLine("An ingredient with that name already exists.");
                return;
            }

            double kcal = ReadDouble("Calories per 100g: ");
            double protein = ReadDouble("Protein per 100g (g): ");
            double carbs = ReadDouble("Carbs per 100g (g): ");
            double fat = ReadDouble("Fat per 100g (g): ");

            var ingredient = new Ingredient(name.Trim(), kcal, protein, carbs, fat);
            db.Ingredients.Add(ingredient);
            db.SaveIngredients();

            Console.WriteLine($"Ingredient '{ingredient.Name}' added.");
        }

        public static void ListIngredients(RecipeDatabase db)
        {
            if (db.Ingredients.Count == 0)
            {
                Console.WriteLine("No ingredients have been added yet.");
                return;
            }

            Console.WriteLine("\nIngredients:");
            int i = 1;
            foreach (var ingredient in db.Ingredients.OrderBy(x => x.Name))
            {
                Console.WriteLine(
                    $"  {i++}. {ingredient.Name} - {ingredient.CaloriesPer100g} kcal / 100g " +
                    $"(P {ingredient.ProteinPer100g}g, C {ingredient.CarbsPer100g}g, F {ingredient.FatPer100g}g)");
            }
        }

        public static Ingredient GetOrCreateIngredientByName(RecipeDatabase db, string name)
        {
            string trimmed = name.Trim();

            var existing = db.Ingredients
                .FirstOrDefault(i => i.Name.Equals(trimmed, StringComparison.OrdinalIgnoreCase));

            if (existing != null)
                return existing;

            Console.WriteLine($"Ingredient '{trimmed}' does not exist yet.");
            Console.WriteLine("Please enter nutritional values per 100g:");

            double kcal = ReadDouble("  Calories: ");
            double protein = ReadDouble("  Protein (g): ");
            double carbs = ReadDouble("  Carbs (g): ");
            double fat = ReadDouble("  Fat (g): ");

            var ingredient = new Ingredient(trimmed, kcal, protein, carbs, fat);
            db.Ingredients.Add(ingredient);
            db.SaveIngredients();

            Console.WriteLine($"Ingredient '{ingredient.Name}' created and added to the database.");
            return ingredient;
        }

        private static double ReadDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (double.TryParse(input, out double value))
                    return value;

                Console.WriteLine("Please enter a valid number.");
            }
        }
    }
}
