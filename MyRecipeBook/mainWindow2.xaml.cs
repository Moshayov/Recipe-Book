using MyRecipeBook.Model;
using MyRecipeBook.ViewModel;
using System;
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

        private void My_Recipes_Click(object sender, RoutedEventArgs e)
        {
            // Create and show a new window (MyRecipesWindow) to display your recipes
            MyRecipesWindow myRecipesWindow = new MyRecipesWindow();
            myRecipesWindow.Show();
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
            bool name = recipe.Title.ToLower().Contains(tbFilter.Text.ToLower());
            return name;
        }

        private void add_to_my_recipes_Click(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedIndex == -1)
            {
                MessageBox.Show("please choose a recipe to add to your recipe book");
            }
            else
            {
                Recipe2 r = viewModel.Recipes[listView.SelectedIndex];
                viewModel.MyRecipes.Add(r.Id);
                MessageBox.Show("The recipe added suucsesfully to your recipe book ");
            }
        }
    }
}
