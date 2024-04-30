using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReceipeGenerator.Model
{
    public class Receipe
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReceipeId { get; set; }
        public string Title { get; set; }
        
       //public List<Ingredient> Ingredients { get; set; }
        public List<RecipeCategory> Categories { get; set; }




    }
}
