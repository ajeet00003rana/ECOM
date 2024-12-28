namespace Project.Models.Api
{
    public class PaymentRequest
    {
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string CVV { get; set; } 
    }

}
