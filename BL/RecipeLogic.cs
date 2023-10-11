using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BL
{
    internal class RecipeLogic
    {
        public List<string> otumtic_recipes()
        {
            List<string> recipes = new List<string>();
            RecipeAdapter g = new RecipeAdapter();
            for (int i = 1; i < 80; i++)
            {
                var recip = g.GetRecipes(i.ToString());
                recipes.Add(recip);
            }
            return recipes;
        }
    }
}
