using FluentAssertions.Execution;
using GetWayServer;
using MyRecipeBook.Model;
using Recipes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using static Recipes.recipe2;

namespace MyRecipeBook.ViewModel
{
    internal class viewModel2 : ViewModelBase
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Recipe2 selectedRecipe = null;
        private Recipe2 newRecipe = null;
        private Recipe2 tempRecipe = null;
        private Byte[] imgeFile = null;
        private FlowDocument comments_rting=null;
        private List<int> rting=new List<int>();
        private ObservableCollection<string> countries = null;
        private ObservableCollection<Recipe2> recipes = null;
        private ObservableCollection<Recipe2> allRecipes = null;
        private ObservableCollection<Recipe2> myRecipes = null;
        public ICommand HomeCommand { get; }
        public ICommand RecipesForBeginnersCommand { get; }
        public ICommand RecommendedRecipesCommand { get; }
        public ICommand SubstitutesCommand { get; }
        public ICommand AboutUsCommand { get; }
        public ICommand SearchCommand { get; }
       // public ICommand PreviousCommand { get; }
       // public ICommand NextCommand { get; }
        private int index = -1;
        private int cbIndex = 0;

        public viewModel2()
        {
            for (int i = 1; i < 6; i++)
            {
                Rting.Add(i);
            }
            Indexs = -1;
            Recipes = get_recipes();
            Countries = new ObservableCollection<string>();
            NewRecipe = new Recipe2();
            AllRecipes = get_recipes();
            MyRecipes = new ObservableCollection<Recipe2>(Recipes.Where(recipe => recipe.Is_Mine == true).ToList());
            // Initialize  commands
            HomeCommand = new DelegateCommand(ExecuteHome);
            RecipesForBeginnersCommand = new DelegateCommand(ExecuteRecipesForBeginners);
            RecommendedRecipesCommand = new DelegateCommand(ExecuteRecommendedRecipes);
            SubstitutesCommand = new DelegateCommand(ExecuteSubstitutesForComponents);
            AboutUsCommand = new DelegateCommand(ExecuteAboutUs);
            SearchCommand = new DelegateCommand(SearchCommandExecute);
           // PreviousCommand = new DelegateCommand(PreviousButtonExecute);
           // NextCommand = new DelegateCommand(NextExecute);     
            Image2 = NewRecipe.ImageSourceToBytes(new JpegBitmapEncoder(), new BitmapImage(new System.Uri(@"C:\Users\USER\source\repos\Recipe-Book\MyRecipeBook\Image\nopreview.jpg"))); ;
        }

        public FlowDocument Comments_rting
        {
            get => comments_rting;
            set => SetProperty(ref comments_rting, value);
        }
        public List<int> Rting
        {
            get => rting;
            set => SetProperty(ref rting, value);
        }

        public Byte[] Image2
        {
            get => imgeFile;
            set => SetProperty(ref imgeFile, value);
        }

        public Byte[] ImgeFile2
        {
            get => imgeFile;
            set => SetProperty(ref imgeFile, value);
        }

        public Byte[]  PreviousButtonExecute()
        {
            if (SelectedRecipe.Indexs >= 0)
            {
                if (SelectedRecipe.Indexs != 0)
                {
                    SelectedRecipe.Indexs--;
                }
                return Recipes.First(r => r.Id == SelectedRecipe.Id).imageFromRecipes[SelectedRecipe.Indexs].ImageFile;
            }
            return null;
           
        }

        public Byte[] NextExecute()
        {
           

            if (SelectedRecipe.Indexs < SelectedRecipe.imageFromRecipes.Count - 1)
                {
                    SelectedRecipe.Indexs++;
                    return SelectedRecipe.imageFromRecipes[SelectedRecipe.Indexs].ImageFile;
                    
                }
            return null;
           
        }

        public ObservableCollection<Recipe2> AllRecipes
        {
            get => allRecipes;
            set => SetProperty(ref allRecipes, value);
        }

        public void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        // Recipes Collection
        public ObservableCollection<Recipe2> Recipes
        {
            get => recipes;
            set => SetProperty(ref recipes, value);
        }
        public int Indexs
        {
            get => index;
            set => SetProperty(ref index, value);
        }

        // Recipes Collection
        public ObservableCollection<Recipe2> MyRecipes
        {
            get => myRecipes;
            set => SetProperty(ref myRecipes, value);
        }

        // Recipes Countries Collection
        public ObservableCollection<string> Countries
        {
            get => countries;
            set => SetProperty(ref countries, value);
        }

        public Recipe2 SelectedRecipe
        {
            get => selectedRecipe;
            set => SetProperty(ref selectedRecipe, value);
        }

        public Recipe2 NewRecipe
        {
            get => newRecipe;
            set => SetProperty(ref newRecipe, value);
        }

        // Temporary recipe for AddEdit form
        public Recipe2 TempRecipe
        {
            get => tempRecipe;
            set => SetProperty(ref tempRecipe, value);
        }
        // Prop for ComboBox SelectedIndex
        public int CbIndex
        {
            get => cbIndex;
            set => SetProperty(ref cbIndex, value);

        }
      
        public ObservableCollection<Recipe2> get_recipes()
        {
            AddRecipe d = new AddRecipe();
           d.UpdateDb();
            using (var dbContext = new RecipeDbContext()) // Create your DbContext instance
            {
                List<recipe2> recipes = dbContext.Recipes.ToList();
                ObservableCollection<Recipe2> recipes2 = new ObservableCollection<Recipe2>();
                    // Query the data from the database
                    //List<recipe2> recipes = dbContext.Recipes.ToList();
                    foreach (recipe2 r1 in recipes)
                    {
                        Recipe2 r2 = d.convert_recipe2ToRecipe2(r1);
                        recipes2.Add(r2);
                    }
                
           
                return recipes2;
            }

        }

      


        public void Back()
        {
            MyRecipes = new ObservableCollection<Recipe2>(AllRecipes.Where(recipe => recipe.Is_Mine == true).ToList());
         
          
        }

        public void Updete()
        {
            Recipes = get_recipes();//every time tht the db chenge we need to updete the view
            MyRecipes= new ObservableCollection<Recipe2>(Recipes.Where(recipe => recipe.Is_Mine == true).ToList());
        }

        private void ExecuteHome(object parameter)
        {
            // Logic for Home button
            // Handle Home command logic

            // Reset the recipes to display all recipes
            ObservableCollection<Recipe2> recipes = new ObservableCollection<Recipe2>();
            foreach(var recipe2 in allRecipes)
            {
                recipes.Add(recipe2);
            }
            Recipes = recipes;

            // Create and show the main window (if not already visible)
            if (Application.Current.MainWindow == null || !(Application.Current.MainWindow is mainWindow2))
            {
                mainWindow2 mainWindow = new mainWindow2();
                Application.Current.MainWindow = mainWindow;
                mainWindow.Show();
            }
        }

        private void ExecuteRecipesForBeginners(object parameter)
        {
            // Logic for Recipes for Beginners button
            // Handle Recipes for Beginners command logic
            // Filter and display easy recipes
            var easyRecipes = Recipes.Where(recipe => recipe.Difficulty == "Easy").ToList();
            Recipes = new ObservableCollection<Recipe2>(easyRecipes);
        }

        private void ExecuteRecommendedRecipes(object parameter)
        {
            // Handle Recommended Recipes command logic
            // Filter and display recommended recipes with an average rating greater than 4
            var recommendedRecipes = Recipes.Where(recipe => recipe.Rating>= 4);
            if (recommendedRecipes == null)
                return;
            Recipes = new ObservableCollection<Recipe2>(recommendedRecipes.ToList());
        }

        private void ExecuteSubstitutesForComponents(object parameter)
        {
            // Logic for Substitutes for Components button
            // Handle Substitutes for Components command logic
            // Create and show a new window (SubstitutesWindow)
            SubstitutesWindow substitutesWindow = new SubstitutesWindow();
            substitutesWindow.Show();
        }

        private void ExecuteAboutUs(object parameter)
        {
            // Logic for About Us button
            // Handle About Us command logic
            // Create and show a new window (AboutUsWindow)
            AboutUsWindow aboutUsWindow = new AboutUsWindow();
            aboutUsWindow.Show();
        }
      
        private void SearchCommandExecute(object parameter)
        {
            // Handle Search command logic
            // Filter search in ListView on text changed in TextBox
            string searchText = parameter as string; // Assuming parameter contains the search text
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                Recipes = new ObservableCollection<Recipe2>(Recipes.Where(recipe => recipe.Title.ToLower().Contains(searchText.ToLower())));
            }
            else
            {
                Recipes = new ObservableCollection<Recipe2>(Recipes); // Show all recipes
            }
        }

        public bool isSimilar(recipe2 recipe1, recipe2 recipe2)
        {
            List<string> title1 = new List<string>();
            List<string> title2 = new List<string>();
            foreach (var word in recipe1.Title.Split(' '))
            {
                title1.Add(word);
            }
            foreach (var word in recipe2.Title.Split(' '))
            {
                title2.Add(word);
            }
            foreach (var word1 in title1)
            {
                foreach (var word2 in title2)
                {
                    if (word1.ToLower().Equals(word2.ToLower()) && word1.ToLower() != "with" && word1.ToLower() != "or" && word1.ToLower() != "to" && word1.ToLower() != "and" && word1.ToLower() != "by")
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void Cenge_MyRecipe()
        {
            ObservableCollection<Recipe2> my_simlir_recipes = new ObservableCollection<Recipe2>();
            foreach(var recipe in MyRecipes)
            {
                if (isSimilar(SelectedRecipe, recipe))
                {
                    my_simlir_recipes.Add(recipe);
                }
            }
            MyRecipes = my_simlir_recipes;
        }
    }
}

