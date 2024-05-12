using SharpDX.WIC;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media.Imaging;
//
namespace Recipes
{
    public class RecipeJson
    {
        public string id { get; set; }
        public string title { get; set; }
        public string difficulty { get; set; }
        public string portion { get; set; }
        public string time { get; set; }
        public string description { get; set; }
        public List<string> ingredients { get; set; }
        public List<Dictionary<string, string>> method { get; set; }
        public string image { get; set; }
    }
    public class recipe2
    {
        public bool Is_Mine { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Ingredients { get; set; }
        public string Comments { get; set; }
        public int Rating { get; set; }
        public string UsageDates { get; set; }
        public List<Images> imageFromRecipes { get; set; }
        public string Difficulty { get; set; }
        public string Portion { get; set; }
        public string Time { get; set; }
        public string Description { get; set; }
        public List<MethodItem> Instructions { get; set; }
        public string Image { get; set; }
        public string Doc { get; set; }
        public byte[] ImageFile { get; set; }
        public string Country { get; set; }
        public byte[] DocumentData { get; set; }//בי
        public byte[] DocumentComment_rting { get; set; }
        public string Doc_Comments { get; set; }
        public int Indexs { get; set; }

        public recipe2() 
        {
            Doc_Comments = string.Empty;
            Is_Mine = false;
            Title = string.Empty;
            Ingredients = string.Empty;
            Comments = string.Empty;
            Rating =0;
            UsageDates = string.Empty;
            Difficulty = "easy";
            Portion = string.Empty;
            Time = string.Empty;
            Description = string.Empty;
            Instructions = new List<MethodItem>();
            Image = string.Empty;
            Doc = string.Empty;
            ImageFile = new byte[0];
            Country = string.Empty;
            DocumentData = new byte[0];
            imageFromRecipes = new List<Images>();
            DocumentComment_rting = new byte[0];
            Indexs = 0;
        }

        public class Images
        {
            public int RecipeId { get; set; }  // Foreign key to Recipe
            public recipe2 Recipe { get; set; }//Navigation property to Recipe
            public byte[] ImageFile { get; set; }
            public int imagId { get; set; }
        }
        public class MethodItem
        {
            public int RecipeId { get; set; }  // Foreign key to Recipe
            public recipe2 Recipe { get; set; }  // Navigation property to Recipe
            public string StepNumber { get; set; } // Step number, e.g., 1, 2, 3
            public string Instruction { get; set; } // The actual instruction text
        }
    

    }
  
}
