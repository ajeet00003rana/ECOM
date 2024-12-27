using System.ComponentModel.DataAnnotations;

namespace Project.Models.EntityModels
{
    public class ShoppingCart
    {
        [Key]
        public int CartId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public Product Product { get; set; }
    }
}
