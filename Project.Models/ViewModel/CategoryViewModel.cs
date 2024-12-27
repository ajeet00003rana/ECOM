namespace Project.Models.ViewModel
{
    public class CategoryViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class UpdateCategoryViewModel : CategoryViewModel
    {
        public int CategoryId { get; set; }
    }
}
