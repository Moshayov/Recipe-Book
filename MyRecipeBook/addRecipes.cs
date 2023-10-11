using GetWayServer;
using MyRecipeBook.Model;
using Newtonsoft.Json;
using Recipes;
using System.Collections.Generic;
using static Recipes.recipe2;
using System.Linq;
using System.Windows.Media.Imaging;
using System;
using System.Data.Entity.Infrastructure;
using static MyRecipeBook.Model.Recipe2;
using System.Windows.Markup;
using System.Windows;
using System.Net;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.IO;

namespace MyRecipeBook
{
    internal class AddRecipe
    {
        public void CreateAndSaveNewRecipe(List<string> recipes)
        {
            foreach(string jsonData in recipes)
            {
                // Create a new Recipe object and set its properties
                RecipeJson recipeJson = JsonConvert.DeserializeObject<RecipeJson>(jsonData);
                // Generate and store the document
                Recipe2 recipe=convert_RecipeJsonToRecipe2(recipeJson);
                recipe.InitializeDefaultDoc();
                try
                {
                    // Save the recipe to the database
                    using (var dbContext = new RecipeDbContext()) // Create your DbContext instance
                    {
                        dbContext.Recipes.Add(recipe);
                         dbContext.SaveChanges();
                    }
                }
                catch (DbUpdateException ex)
                {
                    // Inspect ex.InnerException for more details
                    Console.WriteLine(ex.InnerException.Message);
                }
            }
                    
        }

        public Recipe2 convert_RecipeJsonToRecipe2(RecipeJson recipeJson)
        {

            Recipe2 recipe = new Recipe2();
            recipe.Difficulty = recipeJson.difficulty;
            recipe.Comments = new List<recipe2.Comment>();
            recipe.Time = recipeJson.time;
            recipe.Image = recipeJson.image;
            recipe.Title = recipeJson.title;
            recipe.Description = recipeJson.description;
            recipe.Portion = recipeJson.portion;
            //recipe.Id = int.Parse(recipeJson.id);
            recipe.Ingredients = string.Empty;
            foreach(string ingredien in recipeJson.ingredients)
            {
                recipe.Ingredients += ingredien+"*";
            }
            // Generate and add random ratings to the recipe until the minimum count is reached
            Random rand = new Random();
            int randomRating = rand.Next(1, 10); // Generates a random rating between 1 and 5
            for (int i = 0; i < randomRating; i++)
            {
                int randomRating2 = rand.Next(1, 5);
                recipe.Ratings.Add(new Rating { Stars = randomRating2 });
            }
            List<Dictionary<string, string>> methodList = recipeJson.method;

            recipe.Instructions = methodList
                .SelectMany(dict => dict.Select(kv => new MethodItem
                {
                    StepNumber = kv.Key,
                    Instruction = kv.Value
                }))
                .ToList();
            using (var webClient = new WebClient())//get the imge from the internet
            {
                recipe.ImageFile = webClient.DownloadData(recipe.Image);
            }
            recipe.Comments = new List<Comment>();
            recipe.Ratings = new List<Rating>();
            recipe.UsageDates = new List<UsageDate>();
            // Create a Random object.
            Random random = new Random();
            // Get the number of enum values.
            int numCountries = Enum.GetValues(typeof(Countrys)).Length;
            // Generate a random index within the range of enum values.
            int randomIndex = random.Next(0, numCountries);
            Countrys country = (Countrys)randomIndex;
            recipe.Country = country.ToString();
            // Serialize FlowDocument to binary data
            FlowDocument doc = InitializeDoc(recipe); 
            using (MemoryStream stream = new MemoryStream())
            {
                TextRange range = new TextRange(doc.ContentStart, doc.ContentEnd);
                range.Save(stream, DataFormats.XamlPackage);
                recipe.DocumentData = stream.ToArray();
                recipe.Doc = XamlWriter.Save(doc);
            }           
            return recipe;
        }

        public FlowDocument CreateFlowDocument(RecipeJson recipeJson)
        {
            FlowDocument doc = new FlowDocument();

            // Add Title
            //doc.Blocks.Add(new Paragraph(new Bold(new Run("Title: " + recipeJson.title))));
            // Add other properties like Difficulty, Portion, Time, Description, etc.
            doc.Blocks.Add(new Paragraph(new Bold(new Run($"Description: {recipeJson.description}"))));
            doc.Blocks.Add(new Paragraph(new Bold(new Run($"Difficulty: {recipeJson.difficulty}"))));
            doc.Blocks.Add(new Paragraph(new Bold(new Run($"Portion: {recipeJson.portion}"))));
            doc.Blocks.Add(new Paragraph(new Bold(new Run($"Time: {recipeJson.time}"))));
            // Add Ingredients
            doc.Blocks.Add(new Paragraph(new Bold(new Run("Ingredients:"))));
            List compList = new List();
            doc.Blocks.Add(compList);
            List<Dictionary<string, string>> methodList = recipeJson.method;
            // Create a new list to hold the instruction paragraphs
            List<Paragraph> listItemParagraphs = new List<Paragraph>();
            foreach (var method in methodList)
            {
                foreach (var kv in method)
                {
                    // Construct a paragraph for each instruction
                    var instructionText = $"{kv.Key}: {kv.Value}";
                    var instructionParagraph = new Paragraph(new Bold(new Run(instructionText)));

                    // Add the instruction paragraph to the list
                    listItemParagraphs.Add(instructionParagraph);
                }
            }

            // Add the instruction paragraphs to the FlowDocument
            foreach (var instructionParagraph in listItemParagraphs)
            {
                doc.Blocks.Add(instructionParagraph);
            }
            return doc;
        }

        public void UpdteDb()
        {
            using (var dbContext = new RecipeDbContext()) // Create your DbContext instance
            {
                // Retrieve existing records to update
                List<recipe2> recipesToUpdate = dbContext.Recipes.ToList();
                //(recipe =>  recipe.DocumentData.Length == 0 || recipe.Doc == "" || recipe.Doc.Length == 0) 

                foreach (recipe2 recipeToUpdate in recipesToUpdate)
                {

                    // Generate and add random ratings to the recipe until the minimum count is reached
                    Random rand = new Random();
                    int randomRating = rand.Next(1, 10); // Generates a random rating between 1 and 5
                    for (int i=0;i< randomRating; i++)
                    {
                        int randomRating2 = rand.Next(3, 5);
                        recipeToUpdate.Ratings.Add(new Rating { Stars = randomRating2 });
                    }
                    
                
                    // Create a FlowDocument for the record
                    FlowDocument doc = InitializeDoc(recipeToUpdate);
                    /*
                    using (var webClient = new WebClient())
                    {
                        recipeToUpdate.ImageFile= webClient.DownloadData(recipeToUpdate.Image);
                        //HttpResponseMessage response = await client.GetAsync(r.Image);
                    }
                    */
                    // Serialize FlowDocument to binary data
                    using (MemoryStream stream = new MemoryStream())
                    {
                        TextRange range = new TextRange(doc.ContentStart, doc.ContentEnd);
                        range.Save(stream, DataFormats.XamlPackage);
                        byte[] documentData = stream.ToArray();

                        // Update the DocumentData column for this record
                        recipeToUpdate.DocumentData = documentData;
                        recipeToUpdate.Doc = XamlWriter.Save(doc);
                    }
                }
                // Save changes to the database
                dbContext.SaveChanges();
            }

          
        }

        public List<recipe2> GetRecipes()
        {
            using (var dbContext = new RecipeDbContext()) // Create your DbContext instance
            {
                // Use LINQ to query the database for recipes
                List<recipe2> recipes = dbContext.Recipes.ToList();

                return recipes;
            }
        }

        public Recipe2 convert_recipe2ToRecipe2(recipe2 recipe)
        {
            Recipe2 recipe1 = new Recipe2();
            recipe1.Difficulty = recipe.Difficulty;
            recipe1.Description = recipe.Description;
            recipe1.DocumentData = recipe.DocumentData;
            recipe1.UsageDates = new List<UsageDate>();
            foreach(UsageDate u in recipe.UsageDates)
            {
                recipe1.UsageDates.Add(u);
            }
            //recipe1.UsageDates = recipe.UsageDates;//העתקה רדודה
            recipe1.Ratings = new List<Rating>();
            foreach (Rating r in recipe.Ratings)
            {
                recipe1.Ratings.Add(r);
            }
            recipe1.Comments = recipe.Comments;//
            foreach(Comment c in recipe.Comments)
            {
                recipe1.Comments.Add(c);
            }
            recipe1.Image = recipe.Image;
            recipe1.Country = recipe.Country;
            recipe1.Title = recipe.Title;
            recipe1.Portion = recipe.Portion;
            recipe1.Time = recipe.Time;
            recipe1.Ingredients = recipe.Ingredients;
            recipe1.Instructions =new List<MethodItem>();//
            foreach(MethodItem m in recipe.Instructions)
            {
                recipe1.Instructions.Add(m);
            }
            recipe1.Doc = recipe.Doc;
            recipe1.ImageFile = recipe.ImageFile;
            return recipe1;
        }

        public FlowDocument InitializeDoc(recipe2 r)
        {
            FlowDocument doc = new FlowDocument();

          // Calculate the average rating from the Ratings list
            double averageRating = r.Ratings.Count > 0 ? r.Ratings.Average(rating => rating.Stars) : 0;

            // Get the star rating as a Span with yellow stars
            Span starRatingSpan = GetStarRatingString(averageRating);
            // Create a Paragraph to hold the star rating Span
            Paragraph starRatingParagraph = new Paragraph();
            starRatingParagraph.Inlines.Add(new Bold(new Run($"Rated by: {r.Ratings.Count()} people, ")));
            starRatingParagraph.Inlines.Add(starRatingSpan);
            // Add the star rating Paragraph to the FlowDocument
            doc.Blocks.Add(starRatingParagraph);
            // Add Title
            //doc.Blocks.Add(new Paragraph(new Bold(new Run("Title: " + r.Title))));
            doc.Blocks.Add(new Paragraph(new Bold(new Run($"Description: {r.Description}"))));
            // Add other properties like Difficulty, Portion, Time, Description, etc.
            doc.Blocks.Add(new Paragraph(new Bold(new Run($"Difficulty: {r.Difficulty}"))));
            doc.Blocks.Add(new Paragraph(new Bold(new Run($"Portion: {r.Portion}"))));
            doc.Blocks.Add(new Paragraph(new Bold(new Run($"Time: {r.Time}"))));
            // Add Ingredients
            doc.Blocks.Add(new Paragraph(new Bold(new Run("Ingredients:"))));
           var list_ingredient = r.Ingredients.Split("*");
            foreach (var ingredient in list_ingredient)
            {
                Paragraph ingredientParagraph = new Paragraph(new Run($"- {ingredient.Trim()}"));
                doc.Blocks.Add(ingredientParagraph);
            }
            List compList = new List();
            doc.Blocks.Add(compList);
            // Create a new list to hold the instruction paragraphs
            List<Paragraph> listItemParagraphs = new List<Paragraph>();
            doc.Blocks.Add(new Paragraph(new Bold(new Run("Instruction:"))));
            foreach (var instruction in r.Instructions)
            {
                // Construct a paragraph for each instruction
                var instructionText = $"{instruction.StepNumber}: {instruction.Instruction}";
                var instructionParagraph = new Paragraph(new Run(instructionText));

                // Add the instruction paragraph to the list
                listItemParagraphs.Add(instructionParagraph);
            }

            // Add the instruction paragraphs to the FlowDocument
            foreach (var instructionParagraph in listItemParagraphs)
            {
                doc.Blocks.Add(instructionParagraph);
            }
            // Serialize the FlowDocument to binary data
            using (MemoryStream stream = new MemoryStream())
            {
                TextRange range = new TextRange(doc.ContentStart, doc.ContentEnd);
                range.Save(stream, DataFormats.XamlPackage);
                r.DocumentData = stream.ToArray();
            }
            return doc;
        }

        // Function to generate a yellow star rating string
        private Span GetStarRatingString(double rating)
        {
            int numberOfStars = (int)Math.Round(rating);

            // Create a Span to hold the stars with yellow color
            Span starSpan = new Span();

            for (int i = 0; i < numberOfStars; i++)
            {
                Run starRun = new Run("★");
                starRun.Foreground = Brushes.Red; // Set the color to yellow
                starSpan.Inlines.Add(starRun);
            }

            return starSpan;
        }
    }
}
