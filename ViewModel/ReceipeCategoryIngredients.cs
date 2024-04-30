using System.ComponentModel.DataAnnotations;

namespace ReceipeGenerator.ViewModel
{
    public class ReceipeCategoryIngredients
    {
        public string Name { get; set; }
        public string MeasurementUnit { get; set; }
        public double QuantityPerServing { get; set; }
    }
}
