namespace Models.GeneraQR
{
    public class QREncryptedAdmin
    {
        //public string currency { get; set; }
        //public string gloss { get; set; }
        //public decimal amount { get; set; }
        //public string expirationDate { get; set; }
        //public bool singleUse { get; set; }
        //public string additionalData { get; set; }
        //public string destinationAccountId { get; set; }
        public string codTransaction { get; set; }
        public int codBank { get; set; }
        public CtrnQRData trnQRData { get; set; }
        public QREncryptedAdmin()
        {
            trnQRData = new CtrnQRData();
        }
    }

    public class CtrnQRData
    {
        public string currency { get; set; }
        public string gloss { get; set; }
        public decimal amount { get; set; }
        public string expirationDate { get; set; }
        public bool singleUse { get; set; }
        public string additionalData { get; set; }
        public string destinationAccountId { get; set; }

    }
}
