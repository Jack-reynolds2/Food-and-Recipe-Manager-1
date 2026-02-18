using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FoodAndRecipeManager.Models;

namespace FoodAndRecipeManager.Services
{
    internal class RecipeService
    {
        private readonly RecipeDatabase _db;

        public RecipeService(RecipeDatabase database)
        {
            _db = database;
        }

        public Recipe CreateRecipe(string name, string instructions = "")
        {
            var recipe = new Recipe(name, instructions);
            _db.AddRecipe(recipe);
            return recipe;
        }

        public void AddIngredientToRecipe(Recipe recipe, Ingredient ingredient, double grams)
        {
            var recipeIng = new RecipeIngredient(ingredient, grams);
            recipe.Ingredients.Add(recipeIng);

            _db.SaveRecipes(); // persist immediately
        }

        public List<Recipe> GetAllRecipes()
        {
            return _db.Recipes;
        }

   
        public Recipe? FindByName(string name)
        {
            return _db.Recipes
                      .FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

       
        public bool RemoveRecipe(string name)
        {
            var recipe = FindByName(name);

            if (recipe != null)
            {
                _db.Recipes.Remove(recipe);
                _db.SaveRecipes();
                return true;
            }

            return false;
        }
    }
}

