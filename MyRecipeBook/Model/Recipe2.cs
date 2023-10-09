using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Markup;
using Recipes;
using MyRecipeBook.Converter;
using System.Windows;

namespace MyRecipeBook.Model
{
    public class Recipe2 : recipe2
    {

        public enum Countrys
        {
            UnitedStates,
            Canada,
            UnitedKingdom,
            Australia,
            Germany,
            France,
            Japan,
            China,
            India,
            Brazil
        }

        public Recipe2():base()
        {
            
            ImageFile = ImageSourceToBytes(new JpegBitmapEncoder(), new BitmapImage(new Uri(@"C:\recipeBook\MyRecipeBook\Image\nopreview.jpg")));
            InitializeDefaultDoc();
        }

        public void InitializeDefaultDoc()
        {
            FlowDocument doc = new FlowDocument();

            // Add Title
            doc.Blocks.Add(new Paragraph(new Bold(new Run("Title: " + Title))));

            // Add Ingredients
            doc.Blocks.Add(new Paragraph(new Bold(new Run("Ingredients:"))));
            List compList = new List();
            doc.Blocks.Add(compList);

            // Add Instructions (if needed)
            // doc.Blocks.Add(new Paragraph(new Bold(new Run("Instructions:"))));
            // List instrList = new List();
            // instrList.ListItems.Add(new ListItem(new Paragraph(new Run(Instructions))));
            // doc.Blocks.Add(instrList);

            List<MethodItem> instructions = new List<MethodItem>();
            foreach(MethodItem m in Instructions)
            {
                instructions.Add(m);
            }

            // Create a new list to hold the instruction paragraphs
            List<Paragraph> listItemParagraphs = new List<Paragraph>();
            foreach (var instruction in instructions)
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

            // Add other properties like Difficulty, Portion, Time, Description, etc.
            doc.Blocks.Add(new Paragraph(new Bold(new Run($"Difficulty: {Difficulty}"))));
            doc.Blocks.Add(new Paragraph(new Bold(new Run($"Portion: {Portion}"))));
            doc.Blocks.Add(new Paragraph(new Bold(new Run($"Time: {Time}"))));
            doc.Blocks.Add(new Paragraph(new Bold(new Run($"Description: {Description}"))));

            // Serialize the FlowDocument to binary data
            using (MemoryStream stream = new MemoryStream())
            {
                TextRange range = new TextRange(doc.ContentStart, doc.ContentEnd);
                range.Save(stream, DataFormats.XamlPackage);
                DocumentData = stream.ToArray();
            }
            Doc = XamlWriter.Save(doc);

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


    }

}
