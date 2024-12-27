using System.ComponentModel.DataAnnotations;

namespace Project.Models.EntityModels
{
    public class PaymentLog
    {
        [Key]
        public int PaymentLogId { get; set; }
        public int OrderId { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }

        public Order Order { get; set; }
    }

}
