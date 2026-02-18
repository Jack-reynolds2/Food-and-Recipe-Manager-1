using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodAndRecipeManager.Models
{
    [Serializable]
    internal class Ingredient
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        // per 100g
        public double CaloriesPer100g { get; set; }
        public double ProteinPer100g { get; set; }
        public double CarbsPer100g { get; set; }
        public double FatPer100g { get; set; }


        public Ingredient(string name, double kcal, double protein, double carbs, double fat)
        {
            Id = Guid.NewGuid();
            Name = name;
            CaloriesPer100g = kcal;
            ProteinPer100g = protein;
            CarbsPer100g = carbs;
            FatPer100g = fat;
        }

        public override string ToString()
        {
            return $"{Name} - {CaloriesPer100g} kcal / 100g";
        }
    }
}


