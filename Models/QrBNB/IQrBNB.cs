using Models.GeneraQR;
using System.Threading.Tasks;

namespace Models.QrBNB
{
    public interface IQrBNB
    {
        Task<RespQRData> ObtenerQRData(QREncryptedAdmin idQR, string token);
    }

    public interface ITokenBNB
    {
                Task<respTokenBNB> ObtenerTokenBNB();

    }

}
