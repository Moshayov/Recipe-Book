using GetWayServer;
using MyRecipeBook.Model;
using MyRecipeBook.ViewModel;
using Recipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MyRecipeBook
{
    /// <summary>
    /// Interaction logic for mainWindow2.xaml
    /// </summary>
    public partial class mainWindow2 : Window
    {

        viewModel2 viewModel = new viewModel2();
        public mainWindow2()
        {
            InitializeComponent();

            DataContext = viewModel;
        }

       
        // Filter searche in ListView on text changed in TextBox
        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            listView.Items.Filter = new Predicate<object>(NameFilter);
        }
        // Filtering by recipes name 
        private bool NameFilter(object obj)
        {
            Recipe2 recipe = obj as Recipe2;

            bool name = recipe.Title.ToLower().Contains(tbFilter.Text.ToLower())||recipe.Ingredients.ToLower().Contains(tbFilter.Text.ToLower());
             //bool name = recipe.Title.ToLower().Contains(tbFilter.Text.ToLower());
            return name;
        }

        private void add_to_my_recipes_Click(object sender, RoutedEventArgs e)
        {
            
            if (listView.SelectedIndex == -1)
            {
                MessageBox.Show("please choose a recipe to add to your recipe book");
            }
            else if (viewModel.Recipes[listView.SelectedIndex].Is_Mine == true)
            {
                MessageBox.Show("The recipe is alredy in your recipe book");
            }
            else
            {
                using (var dbContext = new RecipeDbContext()) // Create your DbContext instance
                {
                    recipe2 recipeToUpdate = dbContext.Recipes.FirstOrDefault(r => r.Id == viewModel.Recipes[listView.SelectedIndex].Id);
                    recipeToUpdate.Is_Mine = true;
                    dbContext.SaveChanges();
                }
                viewModel.Recipes[listView.SelectedIndex].Is_Mine = true;
                MessageBox.Show("The recipe added suucsesfully to your recipe book ");
            }
            
        }

        private void ExecuteMyRecipes(object sender, RoutedEventArgs e)
        {
            // Logic for My Recipes button
            // Handle My Recipes command logic
            // Create and show a new window (MyRecipesWindow) to display your recipes
            MyRecipesWindow myRecipesWindow = new MyRecipesWindow();
            myRecipesWindow.DataContext = viewModel;
            myRecipesWindow.Show();
        }

        
    }
}
