using System.ComponentModel.DataAnnotations;

namespace ReceipeGenerator.Model
{
    public class Ingredient
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string MeasurementUnit { get; set; }
        public double QuantityPerServing { get; set; }

    }
}
