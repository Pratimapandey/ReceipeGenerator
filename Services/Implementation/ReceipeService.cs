using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ReceipeGenerator.Data;
using ReceipeGenerator.Model;
using ReceipeGenerator.Services.Interface;
using ReceipeGenerator.ViewModel;
using System.Collections.Generic;
using System.Linq;



namespace ReceipeGenerator.Services.Implementation
{
    public class ReceipeService : IReceipeService
    {
        private readonly ReceipeDbContext _context;

        public ReceipeService(ReceipeDbContext context)
        {
            _context = context;
        }
        public void PopulateFestivals(List<Festival> festivals)
        {
            foreach (var festival in festivals)
            {
                _context.Festivals.Add(festival);
            }
            _context.SaveChanges();
        }

        public List<Receipe> GetAllRecipes()
        {
            return _context.Receipes
                .Include(r => r.Categories) 
                    .ThenInclude(c => c.Ingredients) 
                .ToList();
        }

        public List<IngredientsViewModel> GetAllIngredients()
        {
            return _context.Ingredients.Select(i => new IngredientsViewModel
            {
                Name = i.Name,
                MeasurementUnit = i.MeasurementUnit

            }).ToList();
        }
        public Receipe CreateRecipe(CreateRecipeRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Receipe.Title) || request.Receipe.Categories == null || !request.Receipe.Categories.Any())
            {
                return null;
            }
           
            var existingRecipe = _context.Receipes
     .Include(r => r.Categories)
     .FirstOrDefault(r => r.Title == request.Receipe.Title);

            if (existingRecipe != null)
            {              
                var categoryNamesInDb = existingRecipe.Categories.OrderBy(c => c.Name).Select(c => c.Name).ToList();
                var categoryNamesInRequest = request.Receipe.Categories.OrderBy(c => c.Name).Select(c => c.Name).ToList();

                if (categoryNamesInDb.SequenceEqual(categoryNamesInRequest))
                {
                    return existingRecipe;
                }
            }

            var newRecipe = new Receipe
            {
                Title = request.Receipe.Title,
                Categories = new List<RecipeCategory>()
            };

            foreach (var category in request.Receipe.Categories)
            {
                if (string.IsNullOrEmpty(category.Name) || category.Ingredients == null || !category.Ingredients.Any())
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
                    if (string.IsNullOrEmpty(ingredient.Name) || string.IsNullOrEmpty(ingredient.MeasurementUnit))
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
            _context.Receipes.Add(newRecipe);
            _context.SaveChanges();

            return newRecipe;
        }

        public List<ReceipeCategoryIngredients> GetIngredientsForRecipeTitle(string recipeTitle, string categoryName, int servings)
        {
            var recipe = _context.Receipes
                                  .Include(r => r.Categories)
                                      .ThenInclude(c => c.Ingredients)
                                  .FirstOrDefault(r => r.Title.Trim().ToLower() == recipeTitle.Trim().ToLower());

            if (recipe == null)
            {
                throw new Exception($"Recipe with title '{recipeTitle}' not found.");
            }

            var category = recipe.Categories.FirstOrDefault(c => c.Name.Trim().ToLower() == categoryName.Trim().ToLower());
            if (category == null)
            {
                throw new Exception($"Category '{categoryName}' not found for recipe '{recipeTitle}'.");
            }

            var ingredients = new List<ReceipeCategoryIngredients>();

            foreach (var ingredient in category.Ingredients)
            {
                var clonedIngredient = new ReceipeCategoryIngredients
                {
                    Name = ingredient.Name,
                    MeasurementUnit = ingredient.MeasurementUnit,
                    QuantityPerServing = ingredient.QuantityPerServing * servings
                };

                ingredients.Add(clonedIngredient);
            }

            return ingredients;
        }

        public Receipe UpdateRecipe(Receipe updatedRecipe)
        {
            var existingRecipe = _context.Receipes
                .Include(r => r.Categories)
                    .ThenInclude(c => c.Ingredients)
                .FirstOrDefault(r => r.ReceipeId == updatedRecipe.ReceipeId);

            if (existingRecipe == null)
            {
                return null; 
            }
            existingRecipe.Title = updatedRecipe.Title;
            foreach (var updatedCategory in updatedRecipe.Categories)
            {
                var existingCategory = existingRecipe.Categories.FirstOrDefault(c => c.Id == updatedCategory.Id);
                if (existingCategory == null)
                {
                    existingCategory = new RecipeCategory { Name = updatedCategory.Name };
                    existingRecipe.Categories.Add(existingCategory);
                }
                else
                {
                    existingCategory.Name = updatedCategory.Name;
                }
                foreach (var updatedIngredient in updatedCategory.Ingredients)
                {
                    var existingIngredient = existingCategory.Ingredients.FirstOrDefault(i => i.Id == updatedIngredient.Id);
                    if (existingIngredient == null)
                    {
                        existingIngredient = new Ingredient { Name = updatedIngredient.Name };
                        existingCategory.Ingredients.Add(existingIngredient);
                    }
                    else
                    {
                        existingIngredient.Name = updatedIngredient.Name;
                        existingIngredient.MeasurementUnit = updatedIngredient.MeasurementUnit;
                        existingIngredient.QuantityPerServing = updatedIngredient.QuantityPerServing;
                    }
                }
            }

            _context.SaveChanges();
            return existingRecipe;
        }

        public bool DeleteRecipe(int id)
        {
            var recipeToDelete = _context.Receipes
                .Include(r => r.Categories)
                    .ThenInclude(c => c.Ingredients)
                .FirstOrDefault(r => r.ReceipeId == id);

            if (recipeToDelete == null)
            {
                return false;
            }

            foreach (var category in recipeToDelete.Categories)
            {
                _context.RemoveRange(category.Ingredients);
            }
            _context.RemoveRange(recipeToDelete.Categories);
            _context.Remove(recipeToDelete);

            _context.SaveChanges();
            return true;
        }

    }
}





   