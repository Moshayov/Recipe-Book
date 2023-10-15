using MyRecipeBook.Model;
using System;
using System.Windows;


namespace MyRecipeBook
{
    /// <summary>
    /// Interaction logic for MyRecipesWindow.xaml
    /// </summary>
    public partial class MyRecipesWindow : Window
    {
        public MyRecipesWindow()
        {
            InitializeComponent();


        }
        // Filter searche in ListView on text changed in TextBox
        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
           // listView.Items.Filter = new Predicate<object>(NameFilter);
        }
        // Filtering by recipes name 
        private bool NameFilter(object obj)
        {
            Recipe2 recipe = obj as Recipe2;

            bool name = recipe.Title.ToLower().Contains(tbFilter.Text.ToLower()) || recipe.Ingredients.ToLower().Contains(tbFilter.Text.ToLower());
            //bool name = recipe.Title.ToLower().Contains(tbFilter.Text.ToLower());
            return name;
        }

    }
}
