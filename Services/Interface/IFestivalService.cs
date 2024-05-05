using ReceipeGenerator.Model;
using ReceipeGenerator.ViewModel;

namespace ReceipeGenerator.Services.Interface
{
    public interface IFestivalService
    {
        public List<Festival> GetAllFestivals();
        bool CreateFestival(CreateFestivalRequest request);
        FestivalViewModel GetRecipesAndIngredientsByFestivalName(string festivalName, int servings);
    }
}
