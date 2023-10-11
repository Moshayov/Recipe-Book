using MyRecipeBook.Model;
using Recipes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.ViewModel
{

    public class RecipeRepository
        {
            private GetWayServer.RecipeDbContext _dbContext;

            public RecipeRepository()
            {
                _dbContext = new GetWayServer.RecipeDbContext(); 
            }

            public List<Recipe2> GetRecipes()
            {
                AddRecipe rec = new AddRecipe();
                // Query the data from the database
                 List<recipe2> recipes = _dbContext.Recipes.ToList();
                 List<Recipe2> recipes2 = new List<Recipe2>();
                 foreach (recipe2 r1 in recipes)
                 {
                      Recipe2 r2 = rec.convert_recipe2ToRecipe2(r1);
                       recipes2.Add(r2);
                 }
                  return recipes2;
            }

            // Add other database operations (e.g., AddRecipe, UpdateRecipe, DeleteRecipe) as needed.
        }
    

}
