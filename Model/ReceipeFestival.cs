namespace ReceipeGenerator.Model
{
    public class ReceipeFestival
    {
        public int ReceipeFestivalId { get; set; }
        public int ReceipeId { get; set; }
        public Receipe Receipe { get; set; }

        public int FestivalId { get; set; }
        public Festival Festival { get; set; }
    }
}
