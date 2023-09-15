namespace FBapiService.Models.GeneraQR
{
    public class RespGetQRNotificaction
    {
        public string QRId { get; set; }
        public int statusId { get; set; }
        public string Gloss { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public string? codError { get; set; }
        public string? descError { get; set; }
    }
}
