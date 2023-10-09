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

namespace MyRecipeBook
{
    /// <summary>
    /// Interaction logic for mainWindow2.xaml
    /// </summary>
    public partial class mainWindow2 : Window
    {
        viewModel2 viewModel = null;

        public mainWindow2()
        {
            InitializeComponent();

            viewModel = new viewModel2();

            DataContext = viewModel;
        }

        private void My_Recipes_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
