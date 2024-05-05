using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ReceipeGenerator.Data;
using ReceipeGenerator.Model;
using ReceipeGenerator.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using ReceipeGenerator.Services.Interface;

namespace ReceipeGenerator.Services.Implementation
{
    public class FestivalService: IFestivalService
    {
        private readonly ReceipeDbContext _context;

        public FestivalService(ReceipeDbContext context)
        {
            _context = context;
        }
        public List<Festival> GetAllFestivals()
        {
            return _context.Festivals
                .Include(f => f.Recipes)
                    .ThenInclude(r => r.Categories)
                        .ThenInclude(c => c.Ingredients)
                .ToList();
        }
        public bool CreateFestival(CreateFestivalRequest request)
        {
            try
            {
                if (request == null || request.Festival == null || string.IsNullOrEmpty(request.Festival.Name) || request.Festival.Recipes == null || !request.Festival.Recipes.Any())
                    return false;

                // Check if a festival with the same name already exists
                var existingFestival = _context.Festivals
                    .Include(f => f.Recipes)
                    .FirstOrDefault(f => f.Name.Trim().ToLower() == request.Festival.Name.Trim().ToLower());

                if (existingFestival != null)
                {
                    // Check if the festival has the same recipes
                    var existingRecipeTitles = existingFestival.Recipes.OrderBy(r => r.Title).Select(r => r.Title).ToList();
                    var requestRecipeTitles = request.Festival.Recipes.OrderBy(r => r.Title).Select(r => r.Title).ToList();

                    if (existingRecipeTitles.SequenceEqual(requestRecipeTitles))
                    {
                        // Festival with the same name and recipes already exists
                        return false;
                    }
                }

                var newFestival = new Festival { Name = request.Festival.Name, Recipes = new List<Receipe>() };

                foreach (var recipe in request.Festival.Recipes)
                {
                    if (recipe == null || string.IsNullOrEmpty(recipe.Title) || recipe.Categories == null || !recipe.Categories.Any())
                    {
                        continue;
                    }

                    var newRecipe = new Receipe { Title = recipe.Title, Categories = new List<RecipeCategory>() };

                    foreach (var category in recipe.Categories)
                    {
                        if (category == null || string.IsNullOrEmpty(category.Name) || category.Ingredients == null || !category.Ingredients.Any())
                        {
                            continue;
                        }

                        var newCategory = new RecipeCategory
                        {
                            Name = category.Name,
                            Ingredients = new List<Ingredient>()
                        };

                        foreach (var ingredient in category.Ingredients)
                        {
                            if (ingredient == null || string.IsNullOrEmpty(ingredient.Name) || string.IsNullOrEmpty(ingredient.MeasurementUnit))
                            {
                                Console.WriteLine("Invalid ingredient data encountered.");
                                continue;
                            }

                            try
                            {
                                var existingIngredient = _context.Ingredients.FirstOrDefault(i => i.Name == ingredient.Name);
                                if (existingIngredient == null)
                                {
                                    existingIngredient = new Ingredient
                                    {
                                        Name = ingredient.Name,
                                        MeasurementUnit = ingredient.MeasurementUnit,
                                        QuantityPerServing = ingredient.QuantityPerServing
                                    };
                                    newCategory.Ingredients.Add(existingIngredient);
                                }
                                else
                                {
                                    if (!newCategory.Ingredients.Contains(existingIngredient))
                                    {
                                        newCategory.Ingredients.Add(existingIngredient);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error processing ingredient: {ex.Message}");
                            }
                        }
                        newRecipe.Categories.Add(newCategory);
                    }
                    newFestival.Recipes.Add(newRecipe);
                }
                _context.Festivals.Add(newFestival);
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to create festival: {ex.Message}");
                return false;
            }
        }

        public FestivalViewModel GetRecipesAndIngredientsByFestivalName(string festivalName, int servings)
        {
            try
            {
                Console.WriteLine($"Received festival name: {festivalName}");
                var festival = _context.Festivals
                    .Include(f => f.Recipes)
                        .ThenInclude(r => r.Categories)
                            .ThenInclude(c => c.Ingredients)
                    .FirstOrDefault(f => f.Name == festivalName);

                if (festival == null)
                {
                    Console.WriteLine($"Festival with name '{festivalName}' not found.");
                    return null;
                }
                Console.WriteLine($"Found festival: {festival.Name}");
                var festivalViewModel = new FestivalViewModel
                {
                    Name = festival.Name,
                    Recipes = festival.Recipes?.Select(r => new ReceipeViewModel
                    {
                        Title = r?.Title,
                        Categories = r?.Categories?.Select(c => new RecipeCategoryViewModel
                        {
                            Name = c?.Name,
                            Ingredients = c?.Ingredients?.Select(i => new ReceipeCategoryIngredients
                            {
                                Name = i?.Name,
                                MeasurementUnit = i?.MeasurementUnit,
                                QuantityPerServing = i?.QuantityPerServing * servings ?? 0
                            }).ToList()
                        }).ToList()
                    }).ToList()
                };
                Console.WriteLine($"Constructed festival view model: {JsonConvert.SerializeObject(festivalViewModel)}");
                return festivalViewModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while processing the request: {ex}");
                throw;
            }
        }
    }
}
