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
                .Include(r => r.Ingredients)
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
        public Receipe CreateRecipe(Receipe recipeRequest)
        {

            if (string.IsNullOrEmpty(recipeRequest.Title) || recipeRequest.Ingredients == null || recipeRequest.Ingredients.Count == 0)
            {
                return null;
            }

            var newRecipe = new Receipe
            {

                Title = recipeRequest.Title,
                Ingredients = new List<Ingredient>()
            };

            foreach (var ingredientWithMeasurement in recipeRequest.Ingredients)
            {
                if (ingredientWithMeasurement != null &&
                    !string.IsNullOrEmpty(ingredientWithMeasurement.Name) &&
                    !string.IsNullOrEmpty(ingredientWithMeasurement.MeasurementUnit))
                {
                    try
                    {
                        if (_context != null)
                        {
                            var ingredientName = ingredientWithMeasurement.Name;
                            var measurementUnit = ingredientWithMeasurement.MeasurementUnit;


                            var existingIngredient = _context.Ingredients.FirstOrDefault(i => i.Name == ingredientName);

                            if (existingIngredient != null)
                            {
                                newRecipe.Ingredients.Add(existingIngredient);
                            }
                            else
                            {
                                var newIngredient = new Ingredient
                                {
                                    Name = ingredientName,
                                    MeasurementUnit = measurementUnit
                                };
                                _context.Ingredients.Add(newIngredient);

                                newRecipe.Ingredients.Add(newIngredient);
                            }
                        }
                        else
                        {

                            throw new NullReferenceException("Database context is not available.");
                        }
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine($"Error processing ingredient: {ex.Message}");
                    }
                }
                else
                {

                    Console.WriteLine("Invalid ingredient data encountered.");
                }
            }
            _context.Receipes.Add(newRecipe);
            _context.SaveChanges();
            return newRecipe;
        }

        public List<IngredientsViewModel> GetIngredientsForRecipeTitle(string recipeTitle)
        {
            var normalizedTitle = recipeTitle.ToUpper();

            var recipe = _context.Receipes
                .Include(r => r.Ingredients)
                .FirstOrDefault(r => r.Title.ToUpper() == normalizedTitle);

            if (recipe == null)
            {
                return new List<IngredientsViewModel>();
            }

            var ingredientsViewModel = recipe.Ingredients.Select(i => new IngredientsViewModel
            {
                Name = i.Name,
                MeasurementUnit = i.MeasurementUnit
            }).ToList();

            return ingredientsViewModel;
        }


        public bool CreateFestival(FestivalViewModel festivalData)
        {
            try
            {
                if (festivalData == null || string.IsNullOrEmpty(festivalData.Name) || festivalData.Recipes == null || !festivalData.Recipes.Any())
                    return false;

                var festival = new Festival { Name = festivalData.Name };

                foreach (var recipe in festivalData.Recipes)
                {
                    if (recipe == null || string.IsNullOrEmpty(recipe.Title) || recipe.Ingredients == null || !recipe.Ingredients.Any())
                    {
                        continue;
                    }

                    var newRecipe = new Receipe { Title = recipe.Title };

                    foreach (var ingredient in recipe.Ingredients)
                    {
                        if (string.IsNullOrEmpty(ingredient))
                        {
                            continue;
                        }

                        var existingIngredient = _context.Ingredients.FirstOrDefault(i => i.Name == ingredient);
                        if (existingIngredient != null)
                        {
                            newRecipe.Ingredients.Add(existingIngredient);
                        }
                        else
                        {
                            var newIngredient = new Ingredient { Name = ingredient };
                            _context.Ingredients.Add(newIngredient);
                            newRecipe.Ingredients.Add(newIngredient);
                        }
                    }

                    festival.Recipes.Add(newRecipe);
                    _context.Receipes.Add(newRecipe); // Add recipe to context
                }

                _context.Festivals.Add(festival); // Add festival to context
                _context.SaveChanges(); // Save changes to database
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to create festival: {ex.Message}");
                return false;
            }
        }


        public FestivalViewModel GetRecipesForFestival(string festivalName)
        {
            var festival = _context.Festivals
                .Include(f => f.Recipes)
                .ThenInclude(r => r.Ingredients)
                .FirstOrDefault(f => f.Name.Equals(festivalName, StringComparison.OrdinalIgnoreCase));

            if (festival == null)
            {
                return null;
            }

            var viewModel = new FestivalViewModel
            {
                Name = festival.Name,
                Recipes = festival.Recipes.Select(r => new ReceipeViewModel
                {
                    Title = r.Title,
                    Ingredients = r.Ingredients.Select(i => i.Name).ToList()
                }).ToList()
            };

            return viewModel;
        }


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



   