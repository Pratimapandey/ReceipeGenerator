using ReceipeGenerator.ViewModel;

namespace ReceipeGenerator.Model
{
    public class CreateFestivalRequest
    {
        public int CreateFestivalRequestId { get; set; }
        public Festival Festival { get; set; }
    }
}
