namespace FBapiService.Models.GeneraQRBEC
{
    public class QRNotificationBEC
    {
        public string QRId { get; set; }
        public string transactionId { get; set; }
        public DateTime paymentDate { get; set; }
        public string paymentTime { get; set; }
        public string currency { get; set; }
        public float amount { get; set; }
        public string senderBankCode { get; set; }
        public string senderName { get; set; }
        public string senderDocumentId { get; set; }
        public string senderAccount {get; set; }
    }
}
