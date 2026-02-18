using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodAndRecipeManager.Models
{
    [Serializable]
    internal class Recipe
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Instructions { get; set; }
        public List<RecipeIngredient> Ingredients { get; set; }

        public Recipe(string name, string instructions)
        {
            Id = Guid.NewGuid();
            Name = name;
            Instructions = instructions;
            Ingredients = new List<RecipeIngredient>();
        }

        // Totals computed from the ingredients
        public double TotalCalories => Ingredients.Sum(i => i.Calories);
        public double TotalProtein => Ingredients.Sum(i => i.Protein);
        public double TotalCarbs => Ingredients.Sum(i => i.Carbs);
        public double TotalFat => Ingredients.Sum(i => i.Fat);

        public override string ToString()
        {
            return $"{Name} - {TotalCalories:F0} kcal " +
                   $"(P {TotalProtein:F1}g / C {TotalCarbs:F1}g / F {TotalFat:F1}g)";
        }
    }
}
