using ReceipeGenerator.Model;
using ReceipeGenerator.ViewModel;
using System.Collections.Generic;

namespace ReceipeGenerator.Services.Interface
{
    public interface IReceipeService
    {
        List<Ingredient> GetIngredientsForRecipeTitle(string recipeTitle, int servings);
        Receipe CreateRecipe(Receipe recipeRequest);
        FestivalViewModel GetRecipesForFestival(string festivalName);
        bool CreateFestival(FestivalViewModel festivalData);
    }
}
