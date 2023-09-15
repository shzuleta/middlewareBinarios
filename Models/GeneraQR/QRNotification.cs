namespace FBapiService.Models.GeneraQR
{
    public class QRNotification
    {
        public string QRId { get; set; }
        public string Gloss { get; set; }
        public int sourceBankId { get; set; }
        public string originName { get; set; }
        public string VoucherId { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public string additionalData { get; set; }
   
    }
}
