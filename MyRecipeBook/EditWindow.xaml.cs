using DAL;
using GetWayServer;
using Microsoft.Win32;
using MyRecipeBook.ViewModel;
using Recipes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyRecipeBook
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        public Byte[] ImgeFiles = null;

        public EditWindow()
        {
            InitializeComponent();
        }
        // Filling AddEdit form depending on Add or Edit
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
                ((viewModel2)DataContext).TempRecipe = ((viewModel2)DataContext).SelectedRecipe;      
                rtbCompInstr.Text = ((viewModel2)DataContext).TempRecipe.Comments;
            
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        // Convert BitmapImage to byte array
        public byte[] ImageSourceToBytes(BitmapEncoder encoder, ImageSource imageSource)
        {
            byte[] bytes = null;
            var bitmapSource = imageSource as BitmapSource;

            if (bitmapSource != null)
            {
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    bytes = stream.ToArray();
                }
            }

            return bytes;
        }

        // Creating copy FlowDocument from Xaml string
        public FlowDocument StringToFlowDocument(string str)
        {
            FlowDocument flowDocument = XamlReader.Load(new MemoryStream(Encoding.Default.GetBytes(str))) as FlowDocument;

            return flowDocument;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.jpg; *.png; *.bmp)|*.jpg; *.png; *.bmp";
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                try
                {
                    BitmapImage bitmap = new BitmapImage(new Uri(openFileDialog.FileName));
                    string ext = System.IO.Path.GetExtension(openFileDialog.FileName).ToLower();
                    switch (ext)
                    {
                        case ".jpg":
                            {
                                ImgeFiles = ImageSourceToBytes(new JpegBitmapEncoder(), bitmap);
                            }
                            break;
                        case ".png":
                            {
                                ImgeFiles = ImageSourceToBytes(new PngBitmapEncoder(), bitmap);
                            }
                            break;
                        case ".bmp":
                            {
                   
                                ImgeFiles = ImageSourceToBytes(new BmpBitmapEncoder(), bitmap);
                            }
                            break;
                        default:
                            break;
                    }
                    //if imge not good throw exeption
                    GetWayServer.Controllers.ImaggaController imaggaController = new GetWayServer.Controllers.ImaggaController();
                    string is_good = imaggaController.Get(((viewModel2)DataContext).SelectedRecipe.Title, ImgeFiles);
                    if (is_good != "The image is good")
                    {
                        MessageBox.Show("The image is not releted to the recipe Please upload a new one");
                    }
                    else
                    {
                        BitmapImage image = new BitmapImage();
                        ((viewModel2)DataContext).ImgeFile2 = ImgeFiles;
                        using (MemoryStream memStream = new MemoryStream(((viewModel2)DataContext).ImgeFile2))
                        {
                            image.BeginInit();
                            image.CacheOption = BitmapCacheOption.OnLoad;
                            image.StreamSource = memStream;
                            image.EndInit();
                            image.Freeze();
                        }
                        no_preview.Visibility = Visibility.Hidden;
                        imgRecipe.Source = image;
                        imgRecipe.Visibility = Visibility.Visible;
                    }
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void rating_comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // SelectedIndex = "{Binding CbIndex}"  ItemsSource = "{Binding Rting, UpdateSourceTrigger=PropertyChanged}"
        }

        private void save_button_Click(object sender, RoutedEventArgs e)
        {
            AddRecipe Updete = new AddRecipe();
            using (var dbContext = new RecipeDbContext()) // Create your DbContext instance
            {
                List<recipe2> recipesToUpdate = dbContext.Recipes.ToList();
                recipe2 r = recipesToUpdate.First(r => r.Id == ((viewModel2)DataContext).TempRecipe.Id);
                bool flage = false;
               if ( rtbCompInstr.Text != r.Comments)
                {
                    r.Comments = rtbCompInstr.Text;
                    flage = true;
                }
                
                //sve the dte holidy 
                if (ImgeFiles != null)
                {
                    r.imageFromRecipes.Add(new recipe2.Images { RecipeId = ((viewModel2)DataContext).TempRecipe.Id, ImageFile = ImgeFiles });
                    flage = true;
                }
                if (rating_comboBox.SelectedIndex != -1)
                {
                    r.Rating = rating_comboBox.SelectedIndex + 1;
                    flage = true;
                }
                FlowDocument Doc = Updete.InitializeDoc_rting_commentes(r);

                // Serialize the FlowDocument to binary data
                using (MemoryStream stream = new MemoryStream())
                {
                    TextRange range = new TextRange(Doc.ContentStart, Doc.ContentEnd);
                    range.Save(stream, DataFormats.XamlPackage);
                    r.DocumentComment_rting = stream.ToArray();
                    r.Doc_Comments = XamlWriter.Save(Doc);
                }
                if (flage)
                {
                    HebCalAdapter Hadapter = new HebCalAdapter();
                    GetWayServer.Controllers.HebCalController hebCalController = new GetWayServer.Controllers.HebCalController();
                    List<string> holidy= hebCalController.Get();
                    if (holidy.Contains("No holiday"))
                    {
                        r.UsageDates ="";
                    }
                    else
                    {
                        r.UsageDates = "The recipe is recommended for using in:\r";
                        foreach (string i in holidy)
                        {
                            
                            r.UsageDates += i;
                            r.UsageDates += " ";

                        }
                    }
                    dbContext.SaveChanges();

                    ((viewModel2)DataContext).Updete();//updete so we will be ble to see the comments nd rting
                    Close();
                }
                else
                {
                    MessageBox.Show("You haven't changed anything, if you want to update, update");
                }
               
            }
        }
    }
}
