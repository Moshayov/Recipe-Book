using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Controls;

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
       
    }
}
