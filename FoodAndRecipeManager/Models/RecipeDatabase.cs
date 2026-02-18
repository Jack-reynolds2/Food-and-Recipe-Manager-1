using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FoodAndRecipeManager.Models
{
    [Serializable]
    internal class RecipeDatabase
    {
        private readonly string _ingredientFile = "ingredients.bin";
        private readonly string _recipeFile = "recipes.bin";

        public List<Ingredient> Ingredients { get; private set; }
        public List<Recipe> Recipes { get; private set; }

        public RecipeDatabase()
        {
            Ingredients = new List<Ingredient>();
            Recipes = new List<Recipe>();

            LoadIngredients();
            LoadRecipes();
        }

        // ingredients 

        public void AddIngredient(Ingredient ingredient)
        {
            Ingredients.Add(ingredient);
            SaveIngredients();
        }

        public void SaveIngredients()
        {
            using var fs = File.Open(_ingredientFile, FileMode.Create);
            using var bw = new BinaryWriter(fs);

            bw.Write(Ingredients.Count);

            foreach (var ing in Ingredients)
            {
                bw.Write(ing.Id.ToString());
                bw.Write(ing.Name);
                bw.Write(ing.CaloriesPer100g);
                bw.Write(ing.ProteinPer100g);
                bw.Write(ing.CarbsPer100g);
                bw.Write(ing.FatPer100g);
            }
        }

        public void LoadIngredients()
        {
            if (!File.Exists(_ingredientFile))
                return;

            using var fs = File.Open(_ingredientFile, FileMode.Open);
            using var br = new BinaryReader(fs);

            int count = br.ReadInt32();
            Ingredients = new List<Ingredient>();

            for (int i = 0; i < count; i++)
            {
                var id = Guid.Parse(br.ReadString());
                var name = br.ReadString();
                var kcal = br.ReadDouble();
                var protein = br.ReadDouble();
                var carbs = br.ReadDouble();
                var fat = br.ReadDouble();

                var ing = new Ingredient(name, kcal, protein, carbs, fat)
                {
                    Id = id
                };

                Ingredients.Add(ing);
            }
        }


        //recipes
        public void AddRecipe(Recipe recipe)
        {
            Recipes.Add(recipe);
            SaveRecipes();
        }

        public void SaveRecipes()
        {
            using var fs = File.Open(_recipeFile, FileMode.Create);
            using var bw = new BinaryWriter(fs);

            bw.Write(Recipes.Count);

            foreach (var recipe in Recipes)
            {
                bw.Write(recipe.Id.ToString());
                bw.Write(recipe.Name);
                bw.Write(recipe.Instructions ?? "");

                // write recipe ingredients
                bw.Write(recipe.Ingredients.Count);

                foreach (var ri in recipe.Ingredients)
                {
                    bw.Write(ri.Ingredient.Id.ToString());
                    bw.Write(ri.AmountGrams);
                }
            }
        }


        public void LoadRecipes()
        {
            if (!File.Exists(_recipeFile))
                return;

            using var fs = File.Open(_recipeFile, FileMode.Open);
            using var br = new BinaryReader(fs);

            int count = br.ReadInt32();
            Recipes = new List<Recipe>();

            for (int i = 0; i < count; i++)
            {
                var id = Guid.Parse(br.ReadString());
                var name = br.ReadString();
                var Instructions = br.ReadString();

                var recipe = new Recipe(name, Instructions   )
                {
                    Id = id,
                    Instructions = Instructions
                };

                // Load recipe ingredients
                int ingredientCount = br.ReadInt32();

                for (int j = 0; j < ingredientCount; j++)
                {
                    var ingredientId = Guid.Parse(br.ReadString());
                    var grams = br.ReadDouble();

                    // Look up ingredient from database
                    var ing = Ingredients.FirstOrDefault(x => x.Id == ingredientId);

                    if (ing != null)
                    {
                        recipe.Ingredients.Add(new RecipeIngredient(ing, grams));
                    }
                }

                Recipes.Add((Recipe)recipe);
            }
        }

    }
}
