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
using Microsoft.EntityFrameworkCore; // For Entity Framework Core
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
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
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
            recipe.Comments = string.Empty; ;
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
            int count = 0;
            
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
            recipe.Rating =0;
            recipe.UsageDates = string.Empty;
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

        public void UpdateDb()
        {
            using (var dbContext = new RecipeDbContext()) // Create your DbContext instance
            {
                List<recipe2> recipesToUpdate = dbContext.Recipes.ToList();
                int count = 1;
                foreach (recipe2 recipeToUpdate in recipesToUpdate)
                {
                    dbContext.Entry(recipeToUpdate).State = EntityState.Modified; // Set entity state to Modified

                 //   recipeToUpdate.imageFromRecipes.Add(new Images { RecipeId = recipeToUpdate.Id, ImageFile = recipeToUpdate.ImageFile, Recipe = recipeToUpdate });
              
                    // Create a FlowDocument for the record
                    FlowDocument doc = InitializeDoc(recipeToUpdate);
                    FlowDocument doc1 = InitializeDoc_rting_commentes(recipeToUpdate);
                    // Serialize FlowDocument to binary data
                    using (MemoryStream stream = new MemoryStream())
                    {
                        TextRange range = new TextRange(doc.ContentStart, doc.ContentEnd);
                        range.Save(stream, DataFormats.XamlPackage);
                        byte[] documentData = stream.ToArray();

                        // Update the DocumentData column for this record
                        recipeToUpdate.DocumentData = documentData;
                        recipeToUpdate.Doc = XamlWriter.Save(doc);
                        TextRange range1 = new TextRange(doc1.ContentStart, doc1.ContentEnd);
                        range.Save(stream, DataFormats.XamlPackage);
                        recipeToUpdate.DocumentComment_rting = stream.ToArray();
                        recipeToUpdate.Doc_Comments = XamlWriter.Save(doc1);
                    }
                    
                    try
                    {
                        dbContext.SaveChanges(); // Save changes to the database
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException ex)
                    {
                        // Handle the concurrency exception here, e.g., log it or decide on the appropriate action
                        // You might want to consider refreshing the record from the database and reapplying the changes
                        // or taking another suitable action based on your application's requirements.
                    }

                }
             
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
            recipe1.Is_Mine = recipe.Is_Mine;
            recipe1.Difficulty = recipe.Difficulty;
            recipe1.Description = recipe.Description;
            recipe1.DocumentData = recipe.DocumentData;
            recipe1.Indexs = recipe.Indexs;
            recipe1.Id = recipe.Id;
            //recipe1.Stars = recipe.Stars;
            recipe1.DocumentComment_rting = recipe.DocumentComment_rting;
            recipe1.UsageDates = recipe.UsageDates;
            recipe1.Rating = recipe.Rating;
            recipe1.Comments = recipe.Comments;//
            recipe1.Image = recipe.Image;
            recipe1.Country = recipe.Country;
            recipe1.Title = recipe.Title;
            recipe1.Portion = recipe.Portion;
            recipe1.Time = recipe.Time;
            recipe1.Ingredients = recipe.Ingredients;
            recipe1.Instructions =new List<MethodItem>();//
            recipe1.imageFromRecipes=new List<Images>();//
            recipe1.Doc_Comments = recipe.Doc_Comments;
            foreach (Images ime in recipe.imageFromRecipes)
            {
                recipe1.imageFromRecipes.Add(ime);
            }
            foreach (MethodItem m in recipe.Instructions)
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
            //double averageRating = r.Ratings.Count > 0 ? r.Ratings.Average(rating => rating.Stars) : 0;

            // Get the star rating as a Span with yellow stars
            /*
            if (r.Rating > 0)
            {
                Span starRatingSpan = GetStarRatingString(r.Rating);
                // Create a Paragraph to hold the star rating Span
                Paragraph starRatingParagraph = new Paragraph();
                starRatingParagraph.Inlines.Add(new Bold(new Run($"Rating:")));
                starRatingParagraph.Inlines.Add(starRatingSpan);
                // Add the star rating Paragraph to the FlowDocument
                doc.Blocks.Add(starRatingParagraph);
            }
             */
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

        public FlowDocument InitializeDoc_rting_commentes(recipe2 r)
        {
            FlowDocument doc = new FlowDocument();          
            doc.Blocks.Add(new Paragraph(new Bold(new Run($"What do I think of the recipe 🤔?\r {r.Comments}"))));
            if (r.Rating > 0)
            {
                Span starRatingSpan = GetStarRatingString(r.Rating);
                // Create a Paragraph to hold the star rating Span
                Paragraph starRatingParagraph = new Paragraph();
                starRatingParagraph.Inlines.Add(new Bold(new Run($"Rating:")));
                starRatingParagraph.Inlines.Add(starRatingSpan);
                // Add the star rating Paragraph to the FlowDocument
                doc.Blocks.Add(starRatingParagraph);
            }
            if (r.UsageDates!="")
            {
                doc.Blocks.Add(new Paragraph(new Bold(new Run($"{r.UsageDates}"))));
            }
            // Serialize the FlowDocument to binary data
            using (MemoryStream stream = new MemoryStream())
            {
                TextRange range = new TextRange(doc.ContentStart, doc.ContentEnd);
                range.Save(stream, DataFormats.XamlPackage);
                r.DocumentComment_rting = stream.ToArray();
            }
            return doc;
        }
    }
}
