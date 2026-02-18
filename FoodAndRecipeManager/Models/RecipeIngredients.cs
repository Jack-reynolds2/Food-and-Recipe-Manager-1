using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodAndRecipeManager.Models
{
    [Serializable]
    internal class RecipeIngredient
    {
        public Ingredient Ingredient { get; set; }
        public double AmountGrams { get; set; } // how much of this ingredient in the recipe

        public RecipeIngredient(Ingredient ingredient, double grams)
        {
            Ingredient = ingredient;
            AmountGrams = grams;
        }

        public double Calories => Ingredient.CaloriesPer100g * (AmountGrams / 100.0);
        public double Protein => Ingredient.ProteinPer100g * (AmountGrams / 100.0);
        public double Carbs => Ingredient.CarbsPer100g * (AmountGrams / 100.0);
        public double Fat => Ingredient.FatPer100g * (AmountGrams / 100.0);

        public override string ToString()
        {
            return $"{Ingredient.Name} - {AmountGrams}g";
        }
    }
}

