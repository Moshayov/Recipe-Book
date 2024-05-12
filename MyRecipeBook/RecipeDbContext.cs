using FluentAssertions.Common;
using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Model;
using Recipes;

namespace GetWayServer
{
    public class RecipeDbContext : DbContext
    {
        public DbSet<recipe2> Recipes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=FoodBlog;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<recipe2>(entity =>
            {
                
                // Configure the Method property as owned type (complex type)
                entity.OwnsMany(r => r.imageFromRecipes, method =>
                {
                    method.Property<int>("imagId"); // Foreign key to recipe2
                    method.Property<byte[]>("ImageFile"); // 
                    method.Property<int>("RecipeId").IsRequired(); 
                    // Define the composite key
                    method.HasKey("RecipeId", "imagId");
                });

                // Configure the Method property as owned type (complex type)
                entity.OwnsMany(r => r.Instructions, method =>
                {
                    method.Property<int>("RecipeId"); // Foreign key to recipe2
                    method.Property<string>("StepNumber"); // Step number within the recipe
                    method.Property<string>("Instruction").IsRequired(); // The actual instruction text
                    // Define the composite key
                    method.HasKey("RecipeId", "StepNumber");
                });
            });
        }


    }
}
