using Models.GeneraQR;
using Models.GeneraQRBEC;
using System.Threading.Tasks;

namespace Models.QrBEC
{
    public interface IQrBEC
    {
        Task<RespQRDataBEC> ObtenerQRData(QREncryptedAdminBEC idQR, string token);
    }

    public interface ITokenBEC
    {
                Task<respTokenBEC> ObtenerTokenBEC();

    }

}
