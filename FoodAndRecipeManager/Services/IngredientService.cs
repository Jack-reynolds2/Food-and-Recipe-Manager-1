using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FoodAndRecipeManager.Models;

namespace FoodAndRecipeManager.Services
{
    internal class IngredientService
    {
        private readonly RecipeDatabase _db;

        public IngredientService(RecipeDatabase database)
        {
            _db = database;
        }

        public void AddIngredient(string name, double kcal, double protein, double carbs, double fat)
        {
            var ingredient = new Ingredient(name, kcal, protein, carbs, fat);
            _db.AddIngredient(ingredient);
        }

        public List<Ingredient> GetAllIngredients()
        {
            return _db.Ingredients;
        }

        public Ingredient? FindByName(string name)
        {
            return _db.Ingredients
                      .FirstOrDefault(i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public void RemoveIngredient(string name)
        {
            var ing = FindByName(name);
            if (ing != null)
            {
                _db.Ingredients.Remove(ing);
            }
        }
    }
}

