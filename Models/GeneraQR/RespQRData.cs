namespace Models.GeneraQR
{
    public class respQRData
    {
        public string? id { get; set; }
        public string? qr { get; set; }
        public string? codTransaction { get; set; }
        public bool success { get; set; }
        public string? message { get; set; }
        public string? codError { get; set; } 
        public string? descError { get; set; }

    }
}
