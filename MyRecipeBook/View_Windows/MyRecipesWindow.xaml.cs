using MyRecipeBook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MyRecipeBook.ViewModel;
using System.Windows.Media.Imaging;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.IO;
using Microsoft.EntityFrameworkCore.Metadata;
using Recipes;

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
            listView.Items.Filter = new Predicate<object>(NameFilter);
        }
        // Filtering by recipes name 
        private bool NameFilter(object obj)
        {
            Recipe2 recipe = obj as Recipe2;

            bool name = recipe.Title.ToLower().Contains(tbFilter.Text.ToLower()) || recipe.Ingredients.ToLower().Contains(tbFilter.Text.ToLower());
            //bool name = recipe.Title.ToLower().Contains(tbFilter.Text.ToLower());
            return name;
        }

        private void edit_rting_Click(object sender, RoutedEventArgs e)
        {
            if (listView.SelectedIndex == -1) return;
            EditWindow edit = new EditWindow();
            edit.DataContext = this.DataContext;
            edit.ShowDialog();
            
        }

        private void Find_Similr_Click(object sender, RoutedEventArgs e)
        {
            if(Find_Similr.Content.ToString() == "Find Similar")
            {
                ((viewModel2)DataContext).Cenge_MyRecipe();
                Find_Similr.Content = "Back";
            }
            else
            {
                Find_Similr.Content = "Find Similar";
                ((viewModel2)DataContext).Back();
            }
            
        }

        private void Previus_Click(object sender, RoutedEventArgs e)
        {
            //if imge not good throw exeption
            BitmapImage image = new BitmapImage();
            Byte[] imge = ((viewModel2)DataContext).PreviousButtonExecute();
            ((viewModel2)DataContext).SelectedRecipe.Indexs++;
            if (imge != null)
            {
                using (MemoryStream memStream = new MemoryStream(((viewModel2)DataContext).PreviousButtonExecute()))
                {
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = memStream;
                    image.EndInit();
                    image.Freeze();
                }
                recipeImg.Source = image;
            }
        }
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            //if imge not good throw exeption
            BitmapImage image = new BitmapImage();
            Byte[] imge = ((viewModel2)DataContext).NextExecute();
            ((viewModel2)DataContext).SelectedRecipe.Indexs--;
            if (imge != null)
            {
                using (MemoryStream memStream = new MemoryStream(((viewModel2)DataContext).NextExecute()))
                {
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = memStream;
                    image.EndInit();
                    image.Freeze();
                }
                recipeImg.Source = image;
            }
        }
    }
}
