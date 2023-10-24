using Models.GeneraQR;

namespace FBapiService.Models.GeneraQR
{
    public class QRStatus
    {
        public IDQRStat idQRStat { get; set; }
        public int codClient { get; set; }
        public int codBank { get; set; }
        public string codTransaction { get; set; }
        public string? user { get; set; }

        public QRStatus() 
        {
            idQRStat = new IDQRStat();
        }
    }

    public class IDQRStat
    {
        public string qrId { get; set; }
    }
}
