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
    public class recipe2 : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Ingredients { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Rating> Ratings { get; set; } = new List<Rating>();
        public List<UsageDate> UsageDates { get; set; } = new List<UsageDate>();
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
        public int Stars = 0;

        public recipe2() 
        {
           
            Title = string.Empty;
            Ingredients = string.Empty;
            Comments = new List<Comment>();
            Ratings = new List<Rating>();
            UsageDates = new List<UsageDate>();
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
        }


        public class MethodItem
        {
            public int RecipeId { get; set; }  // Foreign key to Recipe
            public recipe2 Recipe { get; set; }  // Navigation property to Recipe
            public string StepNumber { get; set; } // Step number, e.g., 1, 2, 3
            public string Instruction { get; set; } // The actual instruction text
        }
       

        public class Comment
        {
            public int CommentId { get; set; }
            public string Text { get; set; }
            public int RecipeId { get; set; }
            public recipe2 Recipe { get; set; }
        }

        public class Rating
        {
            public int RatingId { get; set; }
            public int Stars { get; set; }
            public int RecipeId { get; set; }
            public recipe2 Recipe { get; set; }
        }

        public class UsageDate
        {
            public int UsageDateId { get; set; }
            public DateTime Date { get; set; }
            public int RecipeId { get; set; }
            public recipe2 Recipe { get; set; }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
  
}
