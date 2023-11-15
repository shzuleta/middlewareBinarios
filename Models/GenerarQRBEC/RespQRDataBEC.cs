namespace Models.GeneraQRBEC
{
    public class RespQRDataBEC
    {
        public string? qrId { get; set; }
        public string? qrImage { get; set; }
        public string? transactionId { get; set; }
        public string responseCode { get; set; }
        public string? message { get; set; }
        public string? codError { get; set; } 
        public string? descError { get; set; }

    }
}
