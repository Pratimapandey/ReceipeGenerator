using ReceipeGenerator.Model;
using ReceipeGenerator.ViewModel;
using System.Collections.Generic;

namespace ReceipeGenerator.Services.Interface
{
    public interface IReceipeService
    {
        List<IngredientsViewModel> GetIngredientsForRecipeTitle(string recipeTitle);
        Receipe CreateRecipe(Receipe recipeRequest);
        FestivalViewModel GetRecipesForFestival(string festivalName);
        bool CreateFestival(FestivalViewModel festivalData);
    }
}
