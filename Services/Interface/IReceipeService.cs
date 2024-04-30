using ReceipeGenerator.Model;
using ReceipeGenerator.ViewModel;
using System.Collections.Generic;

namespace ReceipeGenerator.Services.Interface
{
    public interface IReceipeService
    {
        List<Receipe> GetAllRecipes();
        List<ReceipeCategoryIngredients> GetIngredientsForRecipeTitle(string recipeTitle, string categoryName, int servings);
        Receipe CreateRecipe(CreateRecipeRequest request);
        Receipe UpdateRecipe(Receipe updatedRecipe);
        bool DeleteRecipe(int id);
        //    FestivalViewModel GetRecipesForFestival(string festivalName);
        //    bool CreateFestival(FestivalViewModel festivalData);
        //}
    }
}
