using System.ComponentModel.DataAnnotations;

namespace ReceipeGenerator.Model
{
    public class RecipeCategory
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Ingredient> Ingredients { get; set; }
    }
}
