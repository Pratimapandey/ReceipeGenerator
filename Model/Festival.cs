namespace ReceipeGenerator.Model
{
    public class Festival
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Season { get; set; }
        public ICollection<Receipe> Recipes { get; set; } // Add this navigation property
    }
}
