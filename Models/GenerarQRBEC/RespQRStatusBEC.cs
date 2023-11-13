using FBapiService.Models.GeneraQR;

namespace FBapiService.Models.GeneraQRBEC
{
    public class RespQRStatusBEC
    {
        public string id { get; set; }
        public int statusQRCode { get; set; } 
        public string? message { get; set; }
        public string? responseCode { get; set; }
        public string? descError { get; set; }
        public List<PaymentQR> payment { get; set; }

        public RespQRStatusBEC()
        {
            payment = new List<PaymentQR>();
        }

    }
    public class PaymentQR
    {
        public string qrId { get; set; }
        public string transactionId { get; set; }
        public DateTime paymentDate { get; set; }
        public string paymentTime { get; set; } 
        public string currency { get; set; }
        public float amount { get; set; }
        public string senderBankCode { get; set; }
        public string senderName { get; set;}
        public string senderDocumentId { get; set; }
        public string senderAccount { get; set; }
    }
}
