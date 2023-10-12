using FluentAssertions.Execution;
using GetWayServer;
using MyRecipeBook.Model;
using Recipes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace MyRecipeBook.ViewModel
{
    internal class viewModel2 : ViewModelBase
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Recipe2 selectedRecipe = null;
        private Recipe2 newRecipe = null;
        private Recipe2 tempRecipe = null;
        private ObservableCollection<string> countries = null;
        private ObservableCollection<Recipe2> recipes = null;
        private ObservableCollection<Recipe2> allRecipes = null;
        public ICommand HomeCommand { get; }
        public ICommand RecipesForBeginnersCommand { get; }
        public ICommand RecommendedRecipesCommand { get; }
        public ICommand SubstitutesCommand { get; }
        public ICommand AboutUsCommand { get; }
        public ICommand SearchCommand { get; }
       // public ICommand addCommand { get; }

        private int cbIndex = 0;

        public viewModel2()
        {
          //  AddRecipe r = new AddRecipe();
          //  r.UpdateDb();
             Recipes = get_recipes();
            Countries = new ObservableCollection<string>();
            NewRecipe = new Recipe2();
            AllRecipes = get_recipes();
            // Initialize your commands
           // addCommand = new DelegateCommand(Executeadd);
            HomeCommand = new DelegateCommand(ExecuteHome);
            RecipesForBeginnersCommand = new DelegateCommand(ExecuteRecipesForBeginners);
            RecommendedRecipesCommand = new DelegateCommand(ExecuteRecommendedRecipes);
            SubstitutesCommand = new DelegateCommand(ExecuteSubstitutesForComponents);
            AboutUsCommand = new DelegateCommand(ExecuteAboutUs);
            SearchCommand = new DelegateCommand(SearchCommandExecute);
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
             //d.UpdateDb();
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
            var recommendedRecipes = Recipes.Where(recipe => recipe.Ratings.Average(rating => rating.Stars) >= 4);
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


    }
}

