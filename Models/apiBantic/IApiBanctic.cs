using Models.GeneraQR;

namespace Models.apiBantic
{
    public interface IApiBanctic
    {
        Task<RespQRData> GetQrData(QRData dataQR);
        //Task<QRQueryResponDTO> QueryQrData(QRQuery dataQR);

        //Task<respConciliacionQR> getConciliacionQR(reqConciliacion dataQR);
    }
}
