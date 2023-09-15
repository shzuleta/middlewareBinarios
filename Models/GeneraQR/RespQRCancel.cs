namespace FBapiService.Models.GeneraQR
{
    public class RespQRCancel
    {
        public string id { get; set; }
        public string idQR { get; set; }
        public string codTransaction { get; set; }
        public bool success { get; set; }
        public string? message { get; set; }
        public string? codError { get; set; }
        public string? descError { get; set; }
    }
}
