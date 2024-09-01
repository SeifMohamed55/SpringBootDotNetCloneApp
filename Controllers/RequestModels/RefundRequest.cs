namespace SpringBootCloneApp.Controllers.RequestModels
{
    public class RefundRequest
    {
        public string TransactionId { get; set; }
        public int AmountCents { get; set; }
        public string RefundReason { get; set; }
    }
}
