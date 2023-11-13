namespace Models.GeneraQR
{
    public class QREncryptedAdminBEC
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
        public CtrnQRDataBEC trnQRData { get; set; }
        public QREncryptedAdminBEC()
        {
            trnQRData = new CtrnQRDataBEC();
        }
    }

    public class CtrnQRDataBEC
    {
        public string transactionId { get; set; }
        public string currency { get; set; }
        public string description { get; set; }
        public decimal amount { get; set; }
        public string dueDate { get; set; }
        public bool singleUse { get; set; }
        public bool modifyAmount { get; set; }
        public string accountCredit { get; set; }

    }
}
