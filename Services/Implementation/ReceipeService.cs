using Microsoft.EntityFrameworkCore;
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
                .Include(r => r.Categories) // Include Categories
                    .ThenInclude(c => c.Ingredients) // Include Ingredients within each Category
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

            var newRecipe = new Receipe
            {
                Title = request.Receipe.Title,
                Categories = new List<RecipeCategory>()
            };

            foreach (var category in request.Receipe.Categories)
            {
                if (string.IsNullOrEmpty(category.Name) || category.Ingredients == null || !category.Ingredients.Any())
                {
                    continue; // Skip categories with missing or empty data
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
                        continue; // Skip ingredients with missing or empty data
                    }

                    try
                    {
                        // Check if the ingredient already exists in the database
                        var existingIngredient = _context.Ingredients.FirstOrDefault(i => i.Name == ingredient.Name);
                        if (existingIngredient == null)
                        {
                            // If the ingredient does not exist, create a new one
                            existingIngredient = new Ingredient
                            {
                                Name = ingredient.Name,
                                MeasurementUnit = ingredient.MeasurementUnit,
                                QuantityPerServing = ingredient.QuantityPerServing
                            };

                            // Add the new ingredient to the category
                            newCategory.Ingredients.Add(existingIngredient);
                        }
                        else
                        {
                            // Add the existing ingredient to the category if not already present
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
                // Add the category to the recipe
                newRecipe.Categories.Add(newCategory);
            }

            _context.Receipes.Add(newRecipe);
            _context.SaveChanges();

            return newRecipe;
        }


        //public void MapReceipeCategoryIngredients(ReceipeCategoryIngredients viewModel, Ingredient model)
        //{
        //    if (viewModel != null && model != null)
        //    {
        //        model.Name = viewModel.Name;
        //        model.MeasurementUnit = viewModel.MeasurementUnit;
        //        model.QuantityPerServing = viewModel.QuantityPerServing;
        //    }
        //}


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
                return null; // Recipe not found
            }

            // Update recipe details
            existingRecipe.Title = updatedRecipe.Title;

            // Update or add categories and ingredients
            foreach (var updatedCategory in updatedRecipe.Categories)
            {
                var existingCategory = existingRecipe.Categories.FirstOrDefault(c => c.Id == updatedCategory.Id);
                if (existingCategory == null)
                {
                    // If category does not exist, create a new one
                    existingCategory = new RecipeCategory { Name = updatedCategory.Name };
                    existingRecipe.Categories.Add(existingCategory);
                }
                else
                {
                    // Update category name if it has changed
                    existingCategory.Name = updatedCategory.Name;
                }

                // Update or add ingredients
                foreach (var updatedIngredient in updatedCategory.Ingredients)
                {
                    var existingIngredient = existingCategory.Ingredients.FirstOrDefault(i => i.Id == updatedIngredient.Id);
                    if (existingIngredient == null)
                    {
                        // If ingredient does not exist, create a new one
                        existingIngredient = new Ingredient { Name = updatedIngredient.Name };
                        existingCategory.Ingredients.Add(existingIngredient);
                    }
                    else
                    {
                        // Update ingredient details if they have changed
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


        //public bool CreateFestival(FestivalViewModel festivalData)
        //{
        //    try
        //    {
        //        if (festivalData == null || string.IsNullOrEmpty(festivalData.Name) || festivalData.Recipes == null || !festivalData.Recipes.Any())
        //            return false;

        //        var festival = new Festival { Name = festivalData.Name };

        //        foreach (var recipe in festivalData.Recipes)
        //        {
        //            if (recipe == null || string.IsNullOrEmpty(recipe.Title) || recipe.Ingredients == null || !recipe.Ingredients.Any())
        //            {
        //                continue;
        //            }

        //            var newRecipe = new Receipe { Title = recipe.Title };

        //            foreach (var ingredient in recipe.Ingredients)
        //            {
        //                if (string.IsNullOrEmpty(ingredient))
        //                {
        //                    continue;
        //                }

        //                var existingIngredient = _context.Ingredients.FirstOrDefault(i => i.Name == ingredient);
        //                if (existingIngredient != null)
        //                {
        //                    newRecipe.Ingredients.Add(existingIngredient);
        //                }
        //                else
        //                {
        //                    var newIngredient = new Ingredient { Name = ingredient };
        //                    _context.Ingredients.Add(newIngredient);
        //                    newRecipe.Ingredients.Add(newIngredient);
        //                }
        //            }

        //            festival.Recipes.Add(newRecipe);
        //            _context.Receipes.Add(newRecipe); // Add recipe to context
        //        }

        //        _context.Festivals.Add(festival); // Add festival to context
        //        _context.SaveChanges(); // Save changes to database
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Failed to create festival: {ex.Message}");
        //        return false;
        //    }
        //}


        //public FestivalViewModel GetRecipesForFestival(string festivalName)
        //{
        //    var festival = _context.Festivals
        //        .Include(f => f.Recipes)
        //        .ThenInclude(r => r.Ingredients)
        //        .FirstOrDefault(f => f.Name.Equals(festivalName, StringComparison.OrdinalIgnoreCase));

        //    if (festival == null)
        //    {
        //        return null;
        //    }

        //    var viewModel = new FestivalViewModel
        //    {
        //        Name = festival.Name,
        //        Recipes = festival.Recipes.Select(r => new ReceipeViewModel
        //        {
        //            Title = r.Title,
        //            Ingredients = r.Ingredients.Select(i => i.Name).ToList()
        //        }).ToList()
        //    };

        //    return viewModel;
        //}


        // Example method in your service layer to recommend festival recipes
        //public List<Receipe> RecommendFestivalRecipes(string season)
        //{
        //    var festivalsForSeason = _context.Festivals
        //        .Include(f => f.Recipes)
        //        .Where(f => f.Season == season)
        //        .ToList();

        //    var recommendedRecipes = new List<Receipe>();
        //    foreach (var festival in festivalsForSeason)
        //    {
        //        recommendedRecipes.AddRange(festival.Recipes);
        //    }

        //    return recommendedRecipes;
        //}
        //// Enhanced GenerateRecipes method to consider festival-specific ingredients
        //public List<Receipe> GenerateRecipes(List<string> ingredients, string festivalName)
        //{
        //    var recipesWithMatchingIngredients = _context.ReceipeFestivals
        //        .Where(rf => rf.Festival.Name == festivalName)
        //        .Select(rf => rf.Receipe)
        //        .Where(recipe => recipe.Ingredients.Any(ingredient => ingredients.Contains(ingredient.Name)))
        //        .ToList();

        //    return recipesWithMatchingIngredients;
        //}




        //// Modify GetIngredientsForRecipe method to also retrieve festival information
        //public Receipe GetRecipeWithFestivals(string recipeTitle)
        //{
        //    var recipe = _context.ReceipeFestivals
        //        .Include(rf => rf.Festival)
        //        .Where(rf => rf.Receipe.Title.Equals(recipeTitle, StringComparison.OrdinalIgnoreCase))
        //        .Select(rf => rf.Receipe)
        //        .FirstOrDefault();

        //    return recipe;
        //}

        //public bool PostRecipesForFestival(string festivalName, List<string> recipeNames)
        //{
        //    if (string.IsNullOrEmpty(festivalName) || recipeNames == null || recipeNames.Count == 0)
        //    {
        //        return false;
        //    }

        //    // Find the festival by name (case-insensitive)
        //    var festival = _context.Festivals.FirstOrDefault(f => f.Name.Equals(festivalName, StringComparison.OrdinalIgnoreCase));
        //    if (festival == null)
        //    {
        //        return false;
        //    }

        //    // Find recipes by their names and include their ingredients
        //    var recipes = _context.Receipes
        //        .Where(r => recipeNames.Contains(r.Title, StringComparer.OrdinalIgnoreCase))
        //        .ToList();

        //    if (recipes == null || !recipes.Any())
        //    {
        //        return false;
        //    }

        //    // Associate recipes with the festival
        //    foreach (var recipe in recipes)
        //    {
        //        _context.ReceipeFestivals.Add(new ReceipeFestival { Receipe = recipe, Festival = festival });
        //    }

        //    _context.SaveChanges();

        //    return true;
        //}
    }
}



   