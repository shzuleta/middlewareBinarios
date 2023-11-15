using FBapiService.Models.GeneraQR;

namespace FBapiService.Models.GeneraQRBEC
{
    public class QRCancelBEC
    {
        public IDQRCancelBEC idQRCancel { get; set; }
            public int codClient { get; set; }
            public int codBank { get; set; }
            public string codTransaction { get; set; }
        public string? user { get; set; }
        public IDQRCancel idQRCancelBEC { get; internal set; }

        public QRCancelBEC()
            {
            idQRCancel = new IDQRCancelBEC();
            }
        


    }

    public class IDQRCancelBEC
    {
        public string qrId { get; set; }
    }
}
