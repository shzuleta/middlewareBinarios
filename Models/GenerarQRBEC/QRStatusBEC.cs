using Models.GeneraQRBEC;

namespace FBapiService.Models.GeneraQRBEC
{
    public class QRStatusBEC
    {
        public IDQRStat idQRStat { get; set; }
        public int codClient { get; set; }
        public int codBank { get; set; }
        public string codTransaction { get; set; }
        public string? user { get; set; }

        public QRStatusBEC() 
        {
            idQRStat = new IDQRStat();
        }
    }

    public class IDQRStat
    {
        public string qrId { get; set; }
    }
}
