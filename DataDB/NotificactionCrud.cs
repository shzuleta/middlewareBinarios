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
                using (var dbContext = new BanticanFintechContext())
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
                using (var dbContext = new BanticanFintechContext())
                {
                    Notification registros1 = new Notification();
                    var registros = dbContext.Notifications.Where(p => p.IdQr == IdQR && p.Status == "0").ToList();

                    if (registros.Count == 1)
                    {
                        // Actualiza el estado de la notificacion como leida
                        Notification dataLog = new Notification();
                        {
                            dataLog.IdQr = IdQR;
                            dataLog.Status = "1";
                        };

                        dbContext.Notifications.Update(dataLog);
                        dbContext.SaveChanges();

                        registros1 = registros.First();

                        return registros1;
                    }
                    else
                    {
                        return "";
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
