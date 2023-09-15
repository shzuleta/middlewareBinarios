using Microsoft.AspNetCore.Components.Web;
using System.Data;
using System.Diagnostics.Eventing.Reader;

namespace FBapiService.DataDB
{
    public class HistoryLogCrud
    {
        public dynamic ActualizarLog(string IdQR, bool Success, string MessageOutput,
                           string JsonOutput, int idLOG, string status) 
        {
            try
            {
                using (var dbContext = new BanticanFintechContext())
                {
                    int IdToUpdate = idLOG; // ID del log que deseas actualizar
                    var LogToUpdate = dbContext.HistoryLogs.Find(IdToUpdate);

                    if (LogToUpdate != null)
                    {
                        LogToUpdate.Daterequest = DateTime.Now;
                        LogToUpdate.IdQr = IdQR;
                        if (Success.Equals(true))
                        { LogToUpdate.Success = "1"; }
                        else
                        { LogToUpdate.Success = "0"; }
                        LogToUpdate.Messageoutput = MessageOutput;
                        LogToUpdate.Jsonoutput = JsonOutput;
                        LogToUpdate.Status = status;
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


        public dynamic RegistrarLog(string Level, string Bank, string Currency, string Gloss, decimal Amount, DateTime ExpirationDate,
                          bool SingleUse, string AdditionalData, string Destinationaccountid, string JsonINput, string IdQR, string Success, string MessageOutput,
                          string JsonOutput, string CodeInter)
        {
            try
            {
                using (var dbContext = new BanticanFintechContext())
                {
                    // Crear una nueva entidad y asignarle valores
                    HistoryLog dataLog = new HistoryLog();
                    {
                        //dataLog.Id = 1;
                        dataLog.Datesend = DateTime.Now;
                        dataLog.Daterequest = DateTime.Now;
                        dataLog.Level = Level;
                        dataLog.Bank = Bank.ToString();
                       

                        dataLog.Currency = Currency;
                        dataLog.Gloss = Gloss;
                        dataLog.Amount = Amount;
                        dataLog.Expirationdate = ExpirationDate;

                        switch (Level)
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
                        dataLog.Messageoutput = MessageOutput;
                        dataLog.Jsonoutput = JsonOutput;
                        dataLog.CodeInter = CodeInter;
                    };

                    dbContext.HistoryLogs.Add(dataLog);
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
