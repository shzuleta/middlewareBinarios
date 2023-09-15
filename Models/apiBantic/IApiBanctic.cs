using Models.GeneraQR;

namespace Models.apiBantic
{
    public interface IApiBanctic
    {
        Task<respQRData> GetQrData(QRData dataQR);
        //Task<QRQueryResponDTO> QueryQrData(QRQuery dataQR);

        //Task<respConciliacionQR> getConciliacionQR(reqConciliacion dataQR);
    }
}
