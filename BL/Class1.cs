using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DP;
namespace BL
{
    internal class Class1
    {
        //stam
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
                                                    //list of usege date
            public List<UsageDate> usageDates { get; set; }

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
            public void func(recipe2 recipe)
            {
                DateTime date = DateTime.Now;
                DateTime date2 = date.AddDays(7);
                recipe.usageDates+(HebCalLogic.)
                   ////////continiue
                
            }
        }
        

    }
}
