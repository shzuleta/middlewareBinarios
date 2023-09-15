using Microsoft.AspNetCore.Components.Web;
using System.Data;
using System.Diagnostics.Eventing.Reader;

namespace FBapiService.DataDB
{
    public class ManageQRCrud
    {
        public dynamic ActualizarManageQR(string IdQR, bool Success, string MessageOutput,
                   string JsonOutput, int idLOG, string status, string typeRequest)
        {
            try
            {
                using (var dbContext = new BanticfintechContext())
                {
                    int IdToUpdate = idLOG; // ID del log que deseas actualizar
                    var LogToUpdate = dbContext.ManageQrs.Find(IdToUpdate);

                    if (LogToUpdate != null)
                    {
                        LogToUpdate.Daterequest = DateTime.Now;
                        LogToUpdate.IdQr = IdQR;
                        if (Success.Equals(true))
                        { LogToUpdate.Success = "1"; }
                        else
                        { LogToUpdate.Success = "0"; }
                        LogToUpdate.Message = MessageOutput;
                        LogToUpdate.Jsonoutput = JsonOutput;
                        if (!typeRequest.Equals("DATAQR"))
                        {
                            LogToUpdate.Status = status;
                        }

                    }

                    dbContext.SaveChanges();

                    return "OK";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public dynamic RegistrarManageQR(string typeRequest, int Bank, string Currency, string Gloss, decimal Amount, DateTime ExpirationDate,
                          bool SingleUse, string AdditionalData, string Destinationaccountid, string JsonINput, string IdQR, string Success, string MessageOutput,
                          string JsonOutput, string codTra, int codClient)
        {
            try
            {
                using (var dbContext = new BanticfintechContext())
                {
                    // Crear una nueva entidad y asignarle valores
                    ManageQr dataLog = new ManageQr();
                    {
                        //dataLog.Id = 1;
                        dataLog.Datesend = DateTime.Now;
                        dataLog.Daterequest = DateTime.Now;
                        dataLog.FkBank = Bank;
                        dataLog.FkCustomer = codClient;

                        dataLog.Currency = Currency;
                        dataLog.Gloss = Gloss;
                        dataLog.Amount = Amount;
                        dataLog.Expirationdate = ExpirationDate;
                        dataLog.Status = "0";
                        dataLog.CodTransaction = codTra;
                        dataLog.TypeRequest = typeRequest;

                        switch (typeRequest)
                        {
                            case "STATUS":
                                dataLog.Singleuse = "-";
                                break;
                            case "CANCEL":
                                dataLog.Singleuse = "-";
                                break;
                            default:
                                if (SingleUse.Equals(true))
                                { dataLog.Singleuse = "1"; }
                                else
                                { dataLog.Singleuse = "0"; }
                                break;
                        }

                        dataLog.Additionaldata = AdditionalData;
                        dataLog.Destinationaccountid = Destinationaccountid;
                        dataLog.Jsoninput = JsonINput;

                        dataLog.IdQr = IdQR;
                        dataLog.Success = Success;
                        dataLog.Message = MessageOutput;
                        dataLog.Jsonoutput = JsonOutput;
                        dataLog.CodTransaction = codTra;

                    };

                    dbContext.ManageQrs.Add(dataLog);
                    dbContext.SaveChanges();

                    return dataLog.Id;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
