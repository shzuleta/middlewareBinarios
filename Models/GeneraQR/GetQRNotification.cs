namespace FBapiService.Models.GeneraQR
{
    public class GetQRNotification
    {
        public string QRId { get; set; }
        public string accountCode { get; set; }
        public int codClient { get; set; }
        public int codBank { get; set; }
        public string codTransaction { get; set; }
    }
}
