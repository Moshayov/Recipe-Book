using MyRecipeBook.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using static Recipes.recipe2;

namespace MyRecipeBook.ViewModel
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private RecipeRepository _recipeRepository;

        private Recipe2 selectedRecipe = null;
        private Recipe2 newRecipe = null;
        private Recipe2 tempRecipe = null;
        private DelegateCommand delCommand = null;
        private DelegateCommand addCommand = null;
        private DelegateCommand refCountries = null;
        private ObservableCollection<Recipe2> recipes = null;
        private ObservableCollection<string> countries = null;
        private ObservableCollection<Comment> comments = null;
        private ObservableCollection<Rating> ratings = null;
        private ObservableCollection<UsageDate> usageDates = null;
        private ObservableCollection<int> myRecipes = null;
        private int cbIndex = 0;

        public MainWindowViewModel()
        {
            Recipes = new ObservableCollection<Recipe2>(_recipeRepository.GetRecipes());
            Countries = new ObservableCollection<string>();
            comments = new ObservableCollection<Comment>();
            ratings = new ObservableCollection<Rating>();
            usageDates = new ObservableCollection<UsageDate>();
            NewRecipe = new Recipe2();
            myRecipes = new ObservableCollection<int>();
        }

        void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // Prop for ComboBox SelectedIndex
        public int CbIndex
        {
            get => cbIndex;
            set
            {
                if (cbIndex != value)
                {
                    cbIndex = value;
                    OnPropertyChanged();
                }
            }
        }

        // Recipes Collection
        public ObservableCollection<Recipe2> Recipes
        {
            get => recipes;
            set
            {
                if (recipes != value)
                {
                    recipes = value;
                    OnPropertyChanged();
                }
            }
        }

        //Comments  Collection
        public ObservableCollection<Comment> Comments
        {
            get => comments;
            set
            {
                if (comments != value)
                {
                    comments = value;
                    OnPropertyChanged();
                }
            }
        }
        //ratings  Collection
        public ObservableCollection<Rating> Ratings
        {
            get => ratings;
            set
            {
                if (ratings != value)
                {
                    ratings = value;
                    OnPropertyChanged();
                }
            }
        }
        //ratings  Collection
        public ObservableCollection<UsageDate> UsageDates
        {
            get => usageDates;
            set
            {
                if (usageDates != value)
                {
                    usageDates = value;
                    OnPropertyChanged();
                }
            }
        }

        //Recipes for my recipe book
        public ObservableCollection<int> MyRecipes
        {
            get => myRecipes;
            set
            {
                if (myRecipes != value)
                {
                    myRecipes = value;
                    OnPropertyChanged();
                }
            }
        }

        // Recipes Countries Collection
        public ObservableCollection<string> Countries
        {
            get => countries;
            set
            {
                if (countries != value)
                {
                    countries = value;
                    OnPropertyChanged();
                }
            }
        }

        public Recipe2 SelectedRecipe
        {
            get => selectedRecipe;
            set
            {
                if (selectedRecipe != value)
                {
                    selectedRecipe = value;
                    OnPropertyChanged();
                }
            }
        }

        public Recipe2 NewRecipe
        {
            get => newRecipe;
            set
            {
                if (newRecipe != value)
                {
                    newRecipe = value;
                    OnPropertyChanged();
                }
            }
        }

        // Temporary recipe for AddEdit form
        public Recipe2 TempRecipe
        {
            get => tempRecipe;
            set
            {
                if (tempRecipe != value)
                {
                    tempRecipe = value;
                    OnPropertyChanged();
                }
            }
        }

        // Delete recipe command
        public DelegateCommand DelCommand
        {
            get => delCommand ?? (delCommand = new DelegateCommand((obj) =>
            {
                Recipes.Remove(SelectedRecipe);
                if (RefCountries.CanExecute(null))
                    RefCountries.Execute(null);
            }));
        }

        // Add recipe command (unused)
        public DelegateCommand AddCommand
        {
            get => addCommand ?? (addCommand = new DelegateCommand((obj) =>
            {
                if (!Recipes.Contains(NewRecipe))
                {
                    Recipes.Add(NewRecipe);
                }
            }));
        }

        // Refresh countries collection command
        public DelegateCommand RefCountries
        {
            get => refCountries ?? (refCountries = new DelegateCommand((obj) =>
            {
                Countries.Clear();
                var uniqCountries = new ObservableCollection<Recipe2>(Recipes.GroupBy(r => r.Country).Select(r => r.FirstOrDefault()));
                Countries.Add("All");
                foreach (Recipe2 country in uniqCountries)
                {
                    if (country.Country != string.Empty)
                        Countries.Add(country.Country);
                }
                CbIndex = 0;
            }));
        }



    
    }
}
