using System;
using System.Linq;
using FoodAndRecipeManager.Models;

namespace FoodAndRecipeManager
{
    internal static class RecipeCommands
    {
        public static void AddRecipeInteractive(RecipeDatabase db)
        {
            Console.WriteLine("\nCreating a new recipe.");

            Console.Write("Recipe name: ");
            string? name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Cancelled: recipe name cannot be empty.");
                return;
            }

            Console.WriteLine("Enter instructions (leave empty for none):");
            string? instructions = Console.ReadLine() ?? string.Empty;

            var recipe = new Recipe(name.Trim(), instructions);

            // Add ingredients to recipe
            if (db.Ingredients.Count == 0)
            {
                Console.WriteLine("No ingredients available. Add some with 'add-ingredient' first.");
                return;
            }

            Console.WriteLine("\nNow add ingredients to the recipe.");
            Console.WriteLine("Type the ingredient number from the list, or 'done' to finish.");

            bool adding = true;
            while (adding)
            {
                ListIngredientsShort(db);

                Console.Write("Ingredient # (or 'done'): ");
                string? input = Console.ReadLine();

                if (string.Equals(input, "done", StringComparison.OrdinalIgnoreCase))
                    break;

                if (!int.TryParse(input, out int index) ||
                    index < 1 || index > db.Ingredients.Count)
                {
                    Console.WriteLine("Invalid selection.");
                    continue;
                }

                var ingredient = db.Ingredients.OrderBy(i => i.Name).ToList()[index - 1];

                double grams = ReadDouble($"Amount of {ingredient.Name} in grams: ");

                var ri = new RecipeIngredient(ingredient, grams);
                recipe.Ingredients.Add(ri);

                Console.WriteLine($"Added {grams}g of {ingredient.Name}.");
            }

            if (recipe.Ingredients.Count == 0)
            {
                Console.WriteLine("No ingredients added. Recipe will not be saved.");
                return;
            }

            db.AddRecipe(recipe); // should call SaveRecipes() internally
            Console.WriteLine($"\nRecipe '{recipe.Name}' added with {recipe.Ingredients.Count} ingredient(s).");
        }

        public static void ListRecipes(RecipeDatabase db)
        {
            if (db.Recipes.Count == 0)
            {
                Console.WriteLine("No recipes have been added yet.");
                return;
            }

            Console.WriteLine("\nRecipes:");
            int i = 1;
            foreach (var recipe in db.Recipes.OrderBy(r => r.Name))
            {
                Console.WriteLine($"  {i++}. {recipe}");
            }
        }

        public static void ShowRecipeInteractive(RecipeDatabase db, string? args)
        {
            string? name = args;

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.Write("Recipe name: ");
                name = Console.ReadLine();
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Recipe name cannot be empty.");
                return;
            }

            var recipe = db.Recipes
                .FirstOrDefault(r => r.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));

            if (recipe == null)
            {
                Console.WriteLine($"No recipe found with name '{name}'.");
                return;
            }

            PrintRecipe(recipe);
        }

        public static void PrintRecipe(Recipe recipe)
        {
            Console.WriteLine($"\n{recipe.Name}");
            Console.WriteLine(new string('-', recipe.Name.Length));

            Console.WriteLine("\nIngredients:");
            if (recipe.Ingredients.Count == 0)
            {
                Console.WriteLine("  [No ingredients]");
            }
            else
            {
                foreach (var ri in recipe.Ingredients)
                {
                    Console.WriteLine(
                        $"  - {ri.Ingredient.Name}: {ri.AmountGrams}g " +
                        $"({ri.Calories:F0} kcal, P {ri.Protein:F1}g, C {ri.Carbs:F1}g, F {ri.Fat:F1}g)");
                }
            }

            Console.WriteLine("\nTotals:");
            Console.WriteLine($"  Calories: {recipe.TotalCalories:F0} kcal");
            Console.WriteLine($"  Protein : {recipe.TotalProtein:F1} g");
            Console.WriteLine($"  Carbs   : {recipe.TotalCarbs:F1} g");
            Console.WriteLine($"  Fat     : {recipe.TotalFat:F1} g");

            Console.WriteLine("\nInstructions:");
            if (string.IsNullOrWhiteSpace(recipe.Instructions))
            {
                Console.WriteLine("  [No instructions]");
            }
            else
            {
                Console.WriteLine(recipe.Instructions);
            }
        }


        private static void ListIngredientsShort(RecipeDatabase db)
        {
            var ordered = db.Ingredients.OrderBy(i => i.Name).ToList();
            Console.WriteLine("\nAvailable ingredients:");
            for (int i = 0; i < ordered.Count; i++)
            {
                Console.WriteLine($"  {i + 1}. {ordered[i].Name} - {ordered[i].CaloriesPer100g} kcal / 100g");
            }
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
