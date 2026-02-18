using System;
using System.Linq;
using FoodAndRecipeManager.Models;

namespace FoodAndRecipeManager
{
    internal static class Program
    {
        private static RecipeDatabase _db = new RecipeDatabase();

        private static void Main(string[] args)
        {
            Console.Title = "Food & Recipe Manager";
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            PrintWelcome();

            bool exitRequested = false;

            while (!exitRequested)
            {
                Console.Write("\n> ");
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                try
                {
                    exitRequested = HandleCommand(input);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                }
            }

            Console.WriteLine("Goodbye!");
        }

        private static void PrintWelcome()
        {
            Console.WriteLine("       FOOD & RECIPE MANAGER     ");
            Console.WriteLine("Type 'help' to see available commands.");
        }

   
        private static bool HandleCommand(string input)
        {
            // command [args...]
            string[] parts = input.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            string command = parts[0].ToLowerInvariant();
            string? args = parts.Length > 1 ? parts[1] : null;

            switch (command)
            {
                case "help":
                case "h":
                    PrintHelp();
                    return false;

                case "add-ingredient":
                case "1":
                    AddIngredientInteractive();
                    return false;

                case "list-ingredients":
                case "2":

                    ListIngredients();
                    return false;

                case "add-recipe":
                case "3":
                    AddRecipeInteractive();
                    return false;

                case "list-recipes":
                case "4":
                    ListRecipes();
                    return false;

                case "show-recipe":
                case "5":
                    ShowRecipeInteractive(args);
                    return false;

                case "clear":
                    Console.Clear();
                    PrintWelcome();
                    return false;

                case "exit":
                case "q":
                    return true;

                case "save":
                    // optional explicit save
                    // data is already being saved when modified
                    _db.SaveIngredients();
                    _db.SaveRecipes();
                    Console.WriteLine("Data saved.");
                    return false;

                default:
                    Console.WriteLine("Unknown command. Type 'help' to see available commands.");
                    return false;
            }
        }

        private static void PrintHelp()
        {
            Console.WriteLine("\nCommands:");
            Console.WriteLine(" 0. help                 - Show this help text");
            Console.WriteLine(" 1. add-ingredient       - Add a new ingredient");
            Console.WriteLine(" 2. list-ingredients     - List all ingredients");
            Console.WriteLine(" 3. add-recipe           - Create a new recipe (with ingredients)");
            Console.WriteLine(" 4. list-recipes         - List all recipes");
            Console.WriteLine(" 5. show-recipe <name>   - Show details of a recipe by name");
            Console.WriteLine(" 6. save                 - Save data to file");
            Console.WriteLine(" 7. clear                - Clear the screen");
            Console.WriteLine(" q. exit                 - Exit the application");
        }

        // ==========================
        // INGREDIENT COMMANDS
        // ==========================

        private static void AddIngredientInteractive()
        {
            Console.WriteLine("\nAdding a new ingredient.");

            Console.Write("Name: ");
            string? name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Cancelled: name cannot be empty.");
                return;
            }

            double kcal = ReadDouble("Calories per 100g: ");
            double protein = ReadDouble("Protein per 100g (g): ");
            double carbs = ReadDouble("Carbs per 100g (g): ");
            double fat = ReadDouble("Fat per 100g (g): ");

            var ingredient = new Ingredient(name, kcal, protein, carbs, fat);
            _db.AddIngredient(ingredient);

            Console.WriteLine($"Ingredient '{name}' added.");
        }

        private static void ListIngredients()
        {
            Console.WriteLine("\nIngredients:");

            if (_db.Ingredients.Count == 0)
            {
                Console.WriteLine("  [No ingredients yet]");
                return;
            }

            int index = 1;
            foreach (var ing in _db.Ingredients)
            {
                Console.WriteLine($"  {index}. {ing.Name} - {ing.CaloriesPer100g} kcal / 100g");
                index++;
            }
        }


        private static void AddRecipeInteractive()
        {
            Console.WriteLine("\nCreating a new recipe.");

            Console.Write("Recipe name: ");
            string? name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Cancelled: name cannot be empty.");
                return;
            }

            Console.WriteLine("Enter instructions (leave empty for none):");
            string? instructions = Console.ReadLine() ?? string.Empty;

            var recipe = new Recipe(name, instructions);

            // Add ingredients loop
            bool adding = true;
            while (adding)
            {
                Console.WriteLine("\nAdd ingredient to recipe? (y/n)");
                string? answer = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(answer) ||
                    answer.StartsWith("n", StringComparison.OrdinalIgnoreCase))
                {
                    adding = false;
                    break;
                }

                if (_db.Ingredients.Count == 0)
                {
                    Console.WriteLine("No ingredients available. Add some with 'add-ingredient' first.");
                    break;
                }

                ListIngredients();
                Console.Write("Enter ingredient number: ");
                string? ingInput = Console.ReadLine();

                if (!int.TryParse(ingInput, out int ingIndex) ||
                    ingIndex < 1 || ingIndex > _db.Ingredients.Count)
                {
                    Console.WriteLine("Invalid ingredient selection.");
                    continue;
                }

                var selectedIngredient = _db.Ingredients[ingIndex - 1];
                double grams = ReadDouble($"Amount of {selectedIngredient.Name} in grams: ");

                var ri = new RecipeIngredient(selectedIngredient, grams);
                recipe.Ingredients.Add(ri);

                Console.WriteLine($"Added {grams}g of {selectedIngredient.Name} to recipe.");
            }

            _db.AddRecipe(recipe);

            Console.WriteLine($"Recipe '{recipe.Name}' added with {recipe.Ingredients.Count} ingredient(s).");
        }

        private static void ListRecipes()
        {
            Console.WriteLine("\nRecipes:");

            if (_db.Recipes.Count == 0)
            {
                Console.WriteLine("  [No recipes yet]");
                return;
            }

            int index = 1;
            foreach (var r in _db.Recipes)
            {
                Console.WriteLine($"  {index}. {r}");
                index++;
            }
        }

        private static void ShowRecipeInteractive(string? args)
        {
            string? name = args;

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.Write("Enter recipe name: ");
                name = Console.ReadLine();
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("No recipe name provided.");
                return;
            }

            var recipe = _db.Recipes
                            .FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (recipe == null)
            {
                Console.WriteLine($"Recipe '{name}' not found.");
                return;
            }

            Console.WriteLine($"\nRecipe: {recipe.Name}");
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Ingredients:");

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
