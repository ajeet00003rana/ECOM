using System.ComponentModel.DataAnnotations;

namespace Project.Models.EntityModels
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; }
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<PaymentLog> PaymentLogs { get; set; }
    }

}
