namespace Models.GeneraQR
{
    public class QRData
    {
        public string accountCode { get; set; }
        public string currency { get; set; }
        public decimal amount { get; set; }
        public int codClient { get; set; }
        public string subCodCliente { get; set; }
        public bool singleUse { get; set; }
        public string expirationDate { get; set; }
        public string clientNote { get; set; }
        public int codBank { get;set; }
        public string codTransaction { get; set; }
    }
}
