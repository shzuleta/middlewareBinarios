namespace FBapiService.DataDB
{
    public class CustomerBankCrud
    {
        public dynamic GetClienteBanco(int client, int bank)
        {
            try
            {
                using (var dbContext = new BanticanFintechContext())
                {
                    // Crear una nueva entidad y asignarle valores
                    CustomerBank dataLog = new CustomerBank();
                    {
                        //dataLog.Id = 1;
                        dataLog.FkBank = bank;
                        dataLog.FkCustomer = client;

                        //dataLog.CodBank = 
                        //dataLog.Destinationaccountid = Destinationaccountid;
                        //dataLog.Jsoninput = JsonINput;

                        //dataLog.IdQr = IdQR;
                        //dataLog.Success = Success;
                        //dataLog.Message = MessageOutput;
                        //dataLog.Jsonoutput = JsonOutput;
                        //dataLog.CodTransaction = codTra;

                    };
                    //dbContext.CustomerBanks.Where()
                    //dbContext.ManageQrs.Add(dataLog);
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
