namespace FBapiService.Models.GeneraQR
{
    public class QRCancel
    {
            public IDQRCancel idQRCancel { get; set; }
            public int codClient { get; set; }
            public int codBank { get; set; }
            public string codTransaction { get; set; }

            public QRCancel()
            {
            idQRCancel = new IDQRCancel();
            }
        


    }

    public class IDQRCancel
    {
        public string qrId { get; set; }
    }
}
