using Microsoft.Win32;
using MyRecipeBook.Model;
using MyRecipeBook.ViewModel;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MyRecipeBook
{
    /// <summary>
    /// Interaction logic for AddEdit.xaml
    /// </summary>
    public partial class AddEdit : Window
    {
        Nullable<bool> addOrEdit = null;

        public AddEdit(bool addOrEdit)
        {
            InitializeComponent();
            this.addOrEdit = addOrEdit;
        }

        // Close AddEdit form without changes
        private void BackBt_Click(object sender, RoutedEventArgs e)
        {
            Close();
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

        // Open file dialog for loading Image from file
        private void OpenImageDialogBt_Click(object sender, RoutedEventArgs e)
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
                            ((MainWindowViewModel)DataContext).TempRecipe.ImageFile = ImageSourceToBytes(new JpegBitmapEncoder(), bitmap);
                            break;
                        case ".png":
                            ((MainWindowViewModel)DataContext).TempRecipe.ImageFile = ImageSourceToBytes(new PngBitmapEncoder(), bitmap);
                            break;
                        case ".bmp":
                            ((MainWindowViewModel)DataContext).TempRecipe.ImageFile = ImageSourceToBytes(new BmpBitmapEncoder(), bitmap);
                            break;
                        default:
                            break;
                    }

                    BitmapImage image = new BitmapImage();
                    using (MemoryStream memStream = new MemoryStream(((MainWindowViewModel)DataContext).TempRecipe.ImageFile))
                    {
                        image.BeginInit();
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.StreamSource = memStream;
                        image.EndInit();
                        image.Freeze();
                    }
                    imgRecipe.Source = image;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // Filling AddEdit form depending on Add or Edit
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (addOrEdit == true)
            {
                ((MainWindowViewModel)DataContext).TempRecipe = ((MainWindowViewModel)DataContext).NewRecipe;
                rtbCompInstr.Document = StringToFlowDocument(((MainWindowViewModel)DataContext).TempRecipe.Doc);
                Title = "Add Recipe";
                //((MainWindowViewModel)DataContext).TempRecipe = ((MainWindowViewModel)DataContext).SelectedRecipe;

            }
            else
            {
                ((MainWindowViewModel)DataContext).TempRecipe = ((MainWindowViewModel)DataContext).SelectedRecipe;

                Title = "Edit Recipe";
                rtbCompInstr.Document = StringToFlowDocument(((MainWindowViewModel)DataContext).TempRecipe.Doc
                    );
            }
        }

        // Compliting adding or editing recipes in recipes collection and closing AddEdit form
        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            if (addOrEdit == true)
            {
                ((MainWindowViewModel)DataContext).TempRecipe.Doc = XamlWriter.Save(rtbCompInstr.Document);
                ((MainWindowViewModel)DataContext).NewRecipe = ((MainWindowViewModel)DataContext).TempRecipe;
                ((MainWindowViewModel)DataContext).Recipes.Add(((MainWindowViewModel)DataContext).NewRecipe);
                
               ((MainWindowViewModel)DataContext).NewRecipe = new Recipe2();
                rtbCompInstr.Document = new FlowDocument();
                if (((MainWindowViewModel)DataContext).RefCountries.CanExecute(null))
                    ((MainWindowViewModel)DataContext).RefCountries.Execute(null);
                Close();
            }
            else
            {
                ((MainWindowViewModel)DataContext).TempRecipe.Doc = XamlWriter.Save(rtbCompInstr.Document);
                ((MainWindowViewModel)DataContext).SelectedRecipe = ((MainWindowViewModel)DataContext).TempRecipe;
                Close();
            }
        }
    }
}
