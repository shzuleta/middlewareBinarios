using Models.GeneraQR;
using System.Threading.Tasks;

namespace Models.QrBNB
{
    public interface IQrBNB
    {
        Task<respQRData> ObtenerQRData(QREncryptedAdmin idQR, string token);
    }

    public interface ITokenBNB
    {
                Task<respTokenBNB> ObtenerTokenBNB();

    }

}
