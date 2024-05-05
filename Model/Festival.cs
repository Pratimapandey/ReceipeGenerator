namespace ReceipeGenerator.Model
{
    public class Festival
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Receipe> Recipes { get; set; }
    }
}
