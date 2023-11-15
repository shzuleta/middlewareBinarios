using Microsoft.EntityFrameworkCore;

namespace FBapiService.DataDB
{
    public class NotificactionCrud
    {
        public dynamic RegistrarNotificationQR(string IdQR,  string Gloss, int souceCodBank, string originName, 
                          string voucherId, DateTime transactionDateTime, string AdditionalData, string type, string status,
                          string usuario)
        {
            try
            {
                using (var dbContext = new BanticfintechContext())
                {
                    // Crear una nueva entidad y asignarle valores
                    Notification dataLog = new Notification();
                    {
                        //dataLog.Id = 1;
                        dataLog.IdQr = IdQR;
                        dataLog.Gloss = Gloss;
                        dataLog.SouceCodBank = souceCodBank.ToString();
                        dataLog.OriginName = originName;
                        dataLog.VoucherId = voucherId;
                        dataLog.TransactionDateTime = transactionDateTime;
                        dataLog.Additionaldata = AdditionalData;
                        dataLog.Type = type;
                        dataLog.Status = "0";
                        dataLog.CreateUser = usuario;
                        dataLog.CreateDate = DateTime.Now;

                    };

                    dbContext.Notifications.Add(dataLog);
                    dbContext.SaveChanges();

                    return dataLog.Id;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public dynamic BuscarNotificationQR(string IdQR, string codClient, string codBank, string codTRansaction)
        {
            try 
            {
                using (var dbContext = new BanticfintechContext())
                {
                    Notification registros1 = new Notification();
                    var registros = dbContext.Notifications.FirstOrDefault(p => p.IdQr == IdQR && p.Status == "0");

                    if (registros != null)
                    {
                        // Actualiza el estado de la notificacion como leida
                        registros.Status = "1";
                        dbContext.SaveChanges();

                        return registros;
                    }
                    else
                    {
                        return "IdQr no encontrado";
                    }
                }

            }
            catch (Exception ex) 
            {
                
                return ex.Message;
            }

        }

        public dynamic BuscarNotificationUser(string IdQR, string usuario)
        {
            try
            {
                using (var dbContext = new BanticfintechContext())
                {
                    Notification registros1 = new Notification();
                    var registros = dbContext.Notifications.FirstOrDefault(p => p.IdQr == IdQR && p.CreateUser == usuario);

                    if (registros != null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {

                return ex.Message;
            }

        }
    }
}
