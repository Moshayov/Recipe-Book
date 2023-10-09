﻿// <auto-generated />
using System;
using GetWayServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MyRecipeBook.Migrations
{
    [DbContext(typeof(RecipeDbContext))]
    partial class RecipeDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Recipes.recipe2", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Difficulty")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Doc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("DocumentData")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("ImageFile")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Ingredients")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Portion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Time")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("Recipes.recipe2+Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CommentId"));

                    b.Property<int>("RecipeId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CommentId");

                    b.HasIndex("RecipeId");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("Recipes.recipe2+Rating", b =>
                {
                    b.Property<int>("RatingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RatingId"));

                    b.Property<int>("RecipeId")
                        .HasColumnType("int");

                    b.Property<int>("Stars")
                        .HasColumnType("int");

                    b.HasKey("RatingId");

                    b.HasIndex("RecipeId");

                    b.ToTable("Rating");
                });

            modelBuilder.Entity("Recipes.recipe2+UsageDate", b =>
                {
                    b.Property<int>("UsageDateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UsageDateId"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("RecipeId")
                        .HasColumnType("int");

                    b.HasKey("UsageDateId");

                    b.HasIndex("RecipeId");

                    b.ToTable("UsageDate");
                });

            modelBuilder.Entity("Recipes.recipe2", b =>
                {
                    b.OwnsMany("Recipes.recipe2+MethodItem", "Instructions", b1 =>
                        {
                            b1.Property<int>("RecipeId")
                                .HasColumnType("int");

                            b1.Property<string>("StepNumber")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("Instruction")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("RecipeId", "StepNumber");

                            b1.ToTable("MethodItem");

                            b1.WithOwner("Recipe")
                                .HasForeignKey("RecipeId");

                            b1.Navigation("Recipe");
                        });

                    b.Navigation("Instructions");
                });

            modelBuilder.Entity("Recipes.recipe2+Comment", b =>
                {
                    b.HasOne("Recipes.recipe2", "Recipe")
                        .WithMany("Comments")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("Recipes.recipe2+Rating", b =>
                {
                    b.HasOne("Recipes.recipe2", "Recipe")
                        .WithMany("Ratings")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("Recipes.recipe2+UsageDate", b =>
                {
                    b.HasOne("Recipes.recipe2", "Recipe")
                        .WithMany("UsageDates")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("Recipes.recipe2", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Ratings");

                    b.Navigation("UsageDates");
                });
#pragma warning restore 612, 618
        }
    }
}
