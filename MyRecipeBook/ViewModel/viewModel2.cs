﻿using MyRecipeBook.Model;
using Recipes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MyRecipeBook.ViewModel
{
    internal class viewModel2 : INotifyPropertyChanged
    {
            public event PropertyChangedEventHandler PropertyChanged;
            private Recipe2 selectedRecipe = null;
            private Recipe2 newRecipe = null;
            private Recipe2 tempRecipe = null;
            private ObservableCollection<string> countries = null;
            private ObservableCollection<Recipe2> recipes = null;
            private ObservableCollection<int> myRecipes = null;
            private int cbIndex = 0;
      
            public viewModel2()
            {
                Recipes = get_recipes();
                Countries = new ObservableCollection<string>();
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

            public ObservableCollection<Recipe2> get_recipes()
        {
            using (var dbContext = new GetWayServer.RecipeDbContext())//get the recipes from the server
            {
                ObservableCollection<Recipe2> recipes2 = new ObservableCollection<Recipe2>();
                try
                {
                    AddRecipe rec = new AddRecipe();
                    // Query the data from the database
                    List<recipe2> recipes = dbContext.Recipes.ToList();
                    foreach (recipe2 r1 in recipes)
                    {
                        Recipe2 r2 = rec.convert_recipe2ToRecipe2(r1);
                        recipes2.Add(r2);
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions here
                    Console.WriteLine(ex.Message);
                }
                return recipes2;
            }

        }
    }

}
