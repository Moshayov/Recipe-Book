using MyRecipeBook.Model;
using MyRecipeBook.ViewModel;
using Recipes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Serialization;
using Windows.UI.Xaml.Controls;

namespace MyRecipeBook
{
    /// <summary>
    /// Interaction logic for mainWindow2.xaml
    /// </summary>
    public partial class mainWindow2 : Window
    {
        

        public mainWindow2()
        {
            InitializeComponent();
          
            viewModel2 viewModel = new viewModel2();

            DataContext = viewModel;
        }

        private void My_Recipes_Click(object sender, RoutedEventArgs e)
        {

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

        private void Recipes_for_beginer_click(object sender, RoutedEventArgs e)
        {
          

        }
    }
}
